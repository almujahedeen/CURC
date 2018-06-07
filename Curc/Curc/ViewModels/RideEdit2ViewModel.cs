using Curc.Enumerations;
using Curc.Helpers;
using Curc.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Curc.ViewModels
{
    public class RideEdit2ViewModel : BaseModel
    {
        #region AutoComplete
        private string _searchText;
        public string searchText {
            get {
                return _searchText;
            }
            set {
                if (_searchText != value) {
                    _searchText = value;
                    this.onPropertyChanged(nameof(searchText));
                    searchTextChanged();
                }
            }
        }
        private List<Prediction> _searchResults;
        public List<Prediction> searchResults {
            get {
                return _searchResults;
            }
            set {
                if (_searchResults != value) {
                    _searchResults = value;
                    this.onPropertyChanged(nameof(searchResults));
                }
            }
        }
        private bool _isListViewVisible;
        public bool isListViewVisible {
            get {
                return _isListViewVisible;
            }
            set {
                if (_isListViewVisible != value) {
                    _isListViewVisible = value;
                    this.onPropertyChanged(nameof(isListViewVisible));
                }
            }
        }
        private ICommand _searchSuggestionSelectedCommand;
        public ICommand searchSuggestionSelectedCommand {
            get {
                return _searchSuggestionSelectedCommand;
            }
            set {
                if (_searchSuggestionSelectedCommand != value) {
                    _searchSuggestionSelectedCommand = value;
                    this.onPropertyChanged(nameof(searchSuggestionSelectedCommand));
                }
            }
        }
        private ICommand _onAppearingCommand;
        public ICommand onAppearingCommand {
            get {
                return _onAppearingCommand;
            }
            set {
                if (_onAppearingCommand != value) {
                    _onAppearingCommand = value;
                    this.onPropertyChanged(nameof(onAppearingCommand));
                }
            }
        }
        private ICommand _onDisappearingCommand;
        public ICommand onDisappearingCommand {
            get {
                return _onDisappearingCommand;
            }
            set {
                if (_onDisappearingCommand != value) {
                    _onDisappearingCommand = value;
                    this.onPropertyChanged(nameof(onDisappearingCommand));
                }
            }
        }
        #endregion

        #region MapOperations
        private MapSpan _visibleRegion;
        public MapSpan visibleRegion {
            get {
                return _visibleRegion;
            }
            set {
                if (_visibleRegion != value) {
                    _visibleRegion = value;
                    this.onPropertyChanged(nameof(visibleRegion));
                }
            }
        }
        private ICommand _moveToRegionCommand;
        public ICommand moveToRegionCommand {
            get {
                return _moveToRegionCommand;
            }
            set {
                if (_moveToRegionCommand != value) {
                    _moveToRegionCommand = value;
                    this.onPropertyChanged(nameof(moveToRegionCommand));
                }
            }
        }
        private ObservableCollection<UserPinModel> _pins;
        public ObservableCollection<UserPinModel> pins {
            get {
                return _pins;
            }
            set {
                if (_pins != value) {
                    _pins = value;
                    this.onPropertyChanged(nameof(pins));
                }
            }
        }
		private ObservableCollection<Polyline> _routes;
		public ObservableCollection<Polyline> routes {
			get { return _routes; }
			set {
				if (_routes != value) {
					_routes = value;
					onPropertyChanged(nameof(routes));
				}
			}
		}
		private ICommand _addStartCommand;
		public ICommand addStartCommand {
            get {
                return _addStartCommand;
            }
            set {
                if (_addStartCommand != value) {
                    _addStartCommand = value;
                    this.onPropertyChanged(nameof(addStartCommand));
                }
            }
        }
        private ICommand _addLegCommand;
        public ICommand addLegCommand {
            get {
                return _addLegCommand;
            }
            set {
                if (_addLegCommand != value) {
                    _addLegCommand = value;
                    this.onPropertyChanged(nameof(addLegCommand));
                }
            }
        }
        private ICommand _addStopoverCommand;
        public ICommand addStopoverCommand {
            get {
                return _addStopoverCommand;
            }
            set {
                if (_addStopoverCommand != value) {
                    _addStopoverCommand = value;
                    this.onPropertyChanged(nameof(addStopoverCommand));
                }
            }
        }
        private ICommand _addEndCommand;
        public ICommand addEndCommand {
            get {
                return _addEndCommand;
            }
            set {
                if (_addEndCommand != value) {
                    _addEndCommand = value;
                    this.onPropertyChanged(nameof(addEndCommand));
                }
            }
        }
        #endregion

        private ICommand _saveCommand;
        public ICommand saveCommand {
            get {
                return _saveCommand;
            }
            set {
                if (_saveCommand != value) {
                    _saveCommand = value;
                    this.onPropertyChanged(nameof(saveCommand));
                }
            }
        }


        public RideEdit2ViewModel(INavigation navigation, RideItemModelFull editableModel)
        {
            pins = new ObservableCollection<UserPinModel>();
			routes = new ObservableCollection<Polyline>();

            saveCommand = new Command(async o => {
                System.Diagnostics.Debug.WriteLine("Saved!");
            });
            onAppearingCommand = new Command(() => {
                scheduleAutoCompletion();
                isOnScreen = true;
            });
            onDisappearingCommand = new Command(() => {
                isOnScreen = false;
            });
            searchSuggestionSelectedCommand = new Command<Prediction>(async p => {
                isListViewVisible = false;

                allowedToShowAutoCompletion = false;
                searchText = p.description;
                allowedToShowAutoCompletion = true;

                await setLocationByPlaceID(p.place_id);
            });
			addStartCommand = new Command(() => {
				addPin(MarkerType.Start);
			});
            addLegCommand = new Command(() => {
                addPin(MarkerType.Leg);
            });
            addStopoverCommand = new Command(() => {
                addPin(MarkerType.StopOver);
            });
            addEndCommand = new Command(() => {
				addPin(MarkerType.End);
            });
        }

        #region AutoComplete
        private bool allowedToShowAutoCompletion = true;
        private DateTime lastInput = DateTime.Now;
        private bool completedPreviousSearch = true;
        private bool isOnScreen;

        private void scheduleAutoCompletion()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(700), () => {
                if (lastInput.Add(TimeSpan.FromMilliseconds(700)) <= DateTime.Now && !completedPreviousSearch) {
                    if (!string.IsNullOrWhiteSpace(searchText))
                        queryAddress();
                    else {
                        isListViewVisible = false;
                    }
                    completedPreviousSearch = true;
                }
                return isOnScreen;
            });
        }

        private async Task searchTextChanged()
        {
            if (!allowedToShowAutoCompletion)
                return;
            lastInput = DateTime.Now;
            completedPreviousSearch = false;
        }

        private async Task queryAddress()
        {
            var result = await API.instance.get<AddressAutocompleteItem>($"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={searchText}&key={Constants.placesAutocompleteAPIKey}");
            if (result?.Item1 == true && result.Item2.predictions?.Count > 0) {
                searchResults = result.Item2.predictions;
                isListViewVisible = true;
            }
        }

        private async Task setLocationByPlaceID(string place_id)
        {
            var detailsResult = await API.instance.get<AddressAutocompletePlaceID>($"https://maps.googleapis.com/maps/api/place/details/json?placeid={place_id}&key={Constants.placesAutocompleteAPIKey}");
            if (detailsResult?.Item1 == true) {
                var location = detailsResult.Item2.result.geometry.location;
                var requestedRegion = MapSpan.FromCenterAndRadius(new Position(location.lat, location.lng), visibleRegion.Radius);
                moveToRegionCommand?.Execute(requestedRegion);
            }
        }
		#endregion

        private void addPin(MarkerType markerType)
        {
            var userPinModel = new UserPinModel {
                name = markerType.ToString(),
                markerType = markerType,
                position = visibleRegion.Center
            };
            pins.Add(userPinModel);

			if (pins.Count - 2 >= 0)
				setRoute(userPinModel.position, pins[pins.Count - 2].position);
        }
		private async Task setRoute(Position? p1, Position? p2)
		{
			if (p1 == null || p2 == null)
				return;
			var query = $"https://maps.googleapis.com/maps/api/directions/json?origin={p1.Value.Latitude},{p1.Value.Longitude}&destination={p2.Value.Latitude},{p2.Value.Longitude}&mode=driving&key={Constants.placesAutocompleteAPIKey}";
			var response = await API.instance.get<GoogleRoute>(query);
			if(response?.Item1==true)
			{
				var positions = response.Item2.routes[0].overview_polyline.polyLinePositions;
				var polyLine = new Polyline {
					StrokeColor = Color.Blue,
					StrokeWidth = 3,
					IsClickable = false
				};
				foreach (var position in positions) {
					polyLine.Positions.Add(position);
				}
				routes.Add(polyLine);
			}
		}
    }
}
