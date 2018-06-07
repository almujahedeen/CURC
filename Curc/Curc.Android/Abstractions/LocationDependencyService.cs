using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Curc.Abstractions;
using Curc.Droid.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationDependencyService))]
namespace Curc.Droid.Abstractions
{
    public class LocationDependencyService : ILocation
    {
        private ICommand locationUpdateCommand;

        public void startLocationUpdate(ICommand locationUpdateCommand)
        {
            this.locationUpdateCommand = locationUpdateCommand;
            LocationHandler.Current.LocationServiceConnected += Current_LocationServiceConnected;
            LocationHandler.StartLocationService();
        }

        public void stopLocationUpdate()
        {
            LocationHandler.Current.LocationServiceConnected -= Current_LocationServiceConnected;
            LocationHandler.Current.LocationService.LocationChanged -= LocationService_LocationChanged;
            LocationHandler.StopLocationService();
        }

        void Current_LocationServiceConnected(object sender, ServiceConnectedEventArgs e)
        {
            LocationHandler.Current.LocationService.LocationChanged += LocationService_LocationChanged;
        }

        void LocationService_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            var position = new Plugin.Geolocator.Abstractions.Position(e.Location.Latitude, e.Location.Longitude);
            position.Accuracy = e.Location.Accuracy;
            position.Altitude = e.Location.Altitude;
            position.Heading = e.Location.Bearing;
            position.Speed = e.Location.Speed;
            DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            try {
                position.Timestamp = new DateTimeOffset(Epoch.AddMilliseconds(e.Location.Time));
            } catch (Exception ex) {
                position.Timestamp = new DateTimeOffset(Epoch);
            }

            locationUpdateCommand?.Execute(position);
        }
    }

    public class LocationHandler
    {
        // events
        public event EventHandler<ServiceConnectedEventArgs> LocationServiceConnected = delegate { };

        // declarations
        protected readonly string logTag = "App";
        protected static LocationServiceConnection locationServiceConnection;

        // properties

        public static LocationHandler Current {
            get { return current; }
        }
        private static LocationHandler current;

        public LocationService LocationService {
            get {
                if (locationServiceConnection.Binder == null)
                    throw new Exception("Service not bound yet");
                // note that we use the ServiceConnection to get the Binder, and the Binder to get the Service here
                return locationServiceConnection.Binder.Service;
            }
        }

        #region Application context
        static LocationHandler()
        {
            current = new LocationHandler();
        }

        protected LocationHandler()
        {
            // create a new service connection so we can get a binder to the service
            locationServiceConnection = new LocationServiceConnection(null);

            // this event will fire when the Service connectin in the OnServiceConnected call 
            locationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {

                Log.Debug(logTag, "Service Connected");
                // we will use this event to notify MainActivity when to start updating the UI
                this.LocationServiceConnected(this, e);
            };
        }

        public static void StartLocationService()
        {
            // Starting a service like this is blocking, so we want to do it on a background thread
            new Task(() => {

                // Start our main service
                Log.Debug("App", "Calling StartService");
                Android.App.Application.Context.StartService(new Intent(Android.App.Application.Context, typeof(LocationService)));

                // bind our service (Android goes and finds the running service by type, and puts a reference
                // on the binder to that service)
                // The Intent tells the OS where to find our Service (the Context) and the Type of Service
                // we're looking for (LocationService)
                Intent locationServiceIntent = new Intent(Android.App.Application.Context, typeof(LocationService));
                Log.Debug("App", "Calling service binding");

                // Finally, we can bind to the Service using our Intent and the ServiceConnection we
                // created in a previous step.
                Android.App.Application.Context.BindService(locationServiceIntent, locationServiceConnection, Bind.AutoCreate);
            }).Start();
        }

        public static void StopLocationService()
        {
            // Check for nulls in case StartLocationService task has not yet completed.
            Log.Debug("App", "StopLocationService");

            // Unbind from the LocationService; otherwise, StopSelf (below) will not work:
            if (locationServiceConnection != null) {
                Log.Debug("App", "Unbinding from LocationService");
                Android.App.Application.Context.UnbindService(locationServiceConnection);
            }

            // Stop the LocationService:
            if (Current.LocationService != null) {
                Log.Debug("App", "Stopping the LocationService");
                Current.LocationService.StopSelf();
            }
        }
        #endregion
    }

    [Service]
    public class LocationService : Service, ILocationListener
    {
        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        public LocationService()
        {
        }

        // Set our location manager as the system location service
        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;

        readonly string logTag = "LocationService";
        IBinder binder;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug(logTag, "OnCreate called in the Location Service");
        }

        // This gets called when StartService is called in our App class
        [Obsolete("deprecated in base class")]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(logTag, "LocationService started");

            return StartCommandResult.Sticky;
        }

        // This gets called once, the first time any client bind to the Service
        // and returns an instance of the LocationServiceBinder. All future clients will
        // reuse the same instance of the binder
        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(logTag, "Client now bound to service");

            binder = new LocationServiceBinder(this);
            return binder;
        }

        // Handle location updates from the location manager
        public void StartLocationUpdates()
        {
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.NoRequirement;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            // get provider: GPS, Network, etc.
            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
            Log.Debug(logTag, string.Format("You are about to get location updates via {0}", locationProvider));

            // Get an initial fix on location
            LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);

            Log.Debug(logTag, "Now sending location updates");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(logTag, "Service has been terminated");

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
        }

        #region ILocationListener implementation
        // ILocationListener is a way for the Service to subscribe for updates
        // from the System location Service

        public void OnLocationChanged(Android.Locations.Location location)
        {
            this.LocationChanged(this, new LocationChangedEventArgs(location));

            // This should be updating every time we request new location updates
            // both when teh app is in the background, and in the foreground
            Log.Debug(logTag, String.Format("Latitude is {0}", location.Latitude));
            Log.Debug(logTag, String.Format("Longitude is {0}", location.Longitude));
            Log.Debug(logTag, String.Format("Altitude is {0}", location.Altitude));
            Log.Debug(logTag, String.Format("Speed is {0}", location.Speed));
            Log.Debug(logTag, String.Format("Accuracy is {0}", location.Accuracy));
            Log.Debug(logTag, String.Format("Bearing is {0}", location.Bearing));
        }

        public void OnProviderDisabled(string provider)
        {
            this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider)
        {
            this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }
        #endregion
    }

    //This is our Binder subclass, the LocationServiceBinder
    public class LocationServiceBinder : Binder
    {
        public LocationService Service {
            get { return this.service; }
        }
        protected LocationService service;

        public bool IsBound { get; set; }

        // constructor
        public LocationServiceBinder(LocationService service)
        {
            this.service = service;
        }
    }

    public class LocationServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };

        public LocationServiceBinder Binder {
            get { return this.binder; }
            set { this.binder = value; }
        }
        protected LocationServiceBinder binder;

        public LocationServiceConnection(LocationServiceBinder binder)
        {
            if (binder != null) {
                this.binder = binder;
            }
        }

        // This gets called when a client tries to bind to the Service with an Intent and an 
        // instance of the ServiceConnection. The system will locate a binder associated with the 
        // running Service 
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            // cast the binder located by the OS as our local binder subclass
            LocationServiceBinder serviceBinder = service as LocationServiceBinder;
            if (serviceBinder != null) {
                this.binder = serviceBinder;
                this.binder.IsBound = true;
                Log.Debug("ServiceConnection", "OnServiceConnected Called");
                // raise the service connected event
                this.ServiceConnected(this, new ServiceConnectedEventArgs() { Binder = service });

                // now that the Service is bound, we can start gathering some location data
                serviceBinder.Service.StartLocationUpdates();
            }
        }

        // This will be called when the Service unbinds, or when the app crashes
        public void OnServiceDisconnected(ComponentName name)
        {
            this.binder.IsBound = false;
            Log.Debug("ServiceConnection", "Service unbound");
        }
    }

    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}