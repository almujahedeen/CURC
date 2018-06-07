using Curc.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.Helpers
{
    public sealed class Location
    {
        private static Location _instance;
        public static Location instance {
            get {
                if (_instance == null)
                    _instance = new Location();
                return _instance;
            }
        }

        private bool _isListening;
        public bool isListening {
            get {
                if (Device.RuntimePlatform == Device.iOS)
                    return CrossGeolocator.Current.IsListening;
                return _isListening;
            }
            set {
                if (_isListening != value)
                    _isListening = value;
            }
        }

        private ILocation locationInstance;
        private ICommand backgroundLocationUpdateCommand;

        private Location()
        {
            if (Device.RuntimePlatform == Device.Android)
                locationInstance = DependencyService.Get<ILocation>();
        }

        public async System.Threading.Tasks.Task<Position> getLocationAsync()
        {
            Position position = null;
            try {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                await askForLocationAccessIfNecessary();

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            } catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
            }

            if (position == null) {
                //TODO: Implement location caching
                //position = Settings.InitalLocation ?? new Position(0, 0);
                position = new Position();
            }
            Debug.WriteLine(position);
            return position;
        }

        public async Task<bool> askForLocationAccessIfNecessary()
        {
            var locator = CrossGeolocator.Current;

            if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled) {
                PermissionStatus status = PermissionStatus.Unknown;
                try {
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                    if (status != PermissionStatus.Granted) {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
                            await App.Current.MainPage.DisplayAlert("Need location", "We need location access", "OK");
                        }

                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Permission.Location))
                            status = results[Permission.Location];
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.StackTrace);
                }
                return status == PermissionStatus.Granted;
            }
            return true;
        }

        public async Task startLocationUpdate(ICommand locationUpdateCommand)
        {
            this.backgroundLocationUpdateCommand = locationUpdateCommand;

            if (!await askForLocationAccessIfNecessary() && isListening)
                return;

            if (Device.RuntimePlatform == Device.iOS) {
                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true, new Plugin.Geolocator.Abstractions.ListenerSettings {
                    ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = false,
                    DeferralDistanceMeters = 1,
                    DeferralTime = TimeSpan.FromSeconds(1),
                    ListenForSignificantChanges = false,
                    PauseLocationUpdatesAutomatically = false
                });
                CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
            } else {
                locationInstance.startLocationUpdate(locationUpdateCommand);
                isListening = true;
            }
        }

        public void stopLocationUpdate()
        {
            //iOS
            CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
            CrossGeolocator.Current.StopListeningAsync();

            //Android
            locationInstance?.stopLocationUpdate();
            isListening = false;
        }

        void Current_PositionChanged(object sender, PositionEventArgs e)
        {
            backgroundLocationUpdateCommand?.Execute(e.Position);
        }
    }
}
