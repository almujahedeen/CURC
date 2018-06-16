using Curc.Extensions;
using Curc.Helpers;
using Curc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Curc.ViewModels
{
    public class OverViewMapViewModel : BindingBase
    {
        public MapSpan visibleRegion { get; set; }
        public Command<MapSpan> moveToRegionCommand { get; set; }

        private ObservableCollection<UserPinModel> _userPins;
        public ObservableCollection<UserPinModel> userPins {
            get {
                return _userPins;
            }
            set {
                if (_userPins != value) {
                    _userPins = value;
                    this.onPropertyChanged(nameof(userPins));
                }
            }
        }

        public OverViewMapViewModel()
        {
            userPins = new ObservableCollection<UserPinModel>();
            loadUsers();
        }

        private void loadUsers()
        {
            Task.Run(async () => {
                var downloadedPins = simulateWS();
                foreach (var pin in downloadedPins) {
					await pin.downloadImageLink();
					Device.BeginInvokeOnMainThread(() => userPins.Add(pin));
                }
            });
        }

        private List<UserPinModel> simulateWS()
        {
            return new List<UserPinModel> {
                new UserPinModel {
                    image = "https://cdn4.iconfinder.com/data/icons/eldorado-user/40/user-128.png",
                    name = "Shit!",
                    position = new Position(41.902783, 12.496366),
                    heading = 90
                },
                new UserPinModel {
                    image = "https://d30y9cdsu7xlg0.cloudfront.net/png/134315-200.png",
                    name = "Missile!",
                    position = new Position(41.852888, 12.568120)
                },
                new UserPinModel {
                    image = "https://d30y9cdsu7xlg0.cloudfront.net/png/996-200.png",
                    name = "Car!",
                    position = new Position(41.853815, 12.420148)
                },
                new UserPinModel {
                    image = "https://d30y9cdsu7xlg0.cloudfront.net/png/21191-200.png",
                    name = "Hello daddy!",
                    position = new Position(41.890629, 12.580136)
                },
                new UserPinModel {
                    image = "https://d30y9cdsu7xlg0.cloudfront.net/png/4254-200.png",
                    name = "Allahuakbar!",
                    position = new Position(41.870179, 12.494992)
                }
            };
        }
    }
}
