using Curc.Models;
using Curc.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.ViewModels
{
    public class RideListViewModel : BaseModel
    {
        public ObservableCollection<RideItemModel> rideList { get; set; }

        private ICommand _itemSelectedCommand;
        public ICommand itemSelectedCommand {
            get {
                return _itemSelectedCommand;
            }
            set {
                if (_itemSelectedCommand != value) {
                    _itemSelectedCommand = value;
                    this.onPropertyChanged(nameof(itemSelectedCommand));
                }
            }
        }

        public RideListViewModel(Page mainNavigation)
        {
            rideList = new ObservableCollection<RideItemModel>();
            init();
            itemSelectedCommand = new Command<RideItemModel>(item => {
                //Manual downcast
                var model = new RideItemModelFull {
                    rideName = item.rideName,
                    rideDate = item.rideDate
                };
                model.mapViewModel = new MapViewModel(mainNavigation.Navigation, model);

                var mapPage = new MapPage();
                mapPage.BindingContext = model;
                mainNavigation.Navigation.PushAsync(mapPage);
            });

        }

        private async Task init()
        {
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu Cebu ",
                rideEndLocation = "Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander Santander ",
                rideDate = DateTime.Now
            });
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu",
                rideEndLocation = "Santander",
                rideDate = DateTime.Now
            });
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu",
                rideEndLocation = "Santander",
                rideDate = DateTime.Now
            });
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu",
                rideEndLocation = "Santander",
                rideDate = DateTime.Now
            });
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu",
                rideEndLocation = "Santander",
                rideDate = DateTime.Now
            });
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu",
                rideEndLocation = "Santander",
                rideDate = DateTime.Now
            });
            rideList.Add(new RideItemModel {
                rideName = "Test Ride",
                rideStartLocation = "Cebu",
                rideEndLocation = "Santander",
                rideDate = DateTime.Now
            });
        }
    }
}
