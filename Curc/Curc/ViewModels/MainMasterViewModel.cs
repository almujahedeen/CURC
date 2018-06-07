using Curc.Models;
using Curc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.ViewModels
{
    class MainMasterViewModel : BindingBase
    {
        private bool _isSideMenuPresented;
        public bool isSideMenuPresented {
            get {
                return _isSideMenuPresented;
            }
            set {
                if (_isSideMenuPresented != value) {
                    _isSideMenuPresented = value;
                    this.onPropertyChanged(nameof(isSideMenuPresented));
                }
            }
        }

        private ICommand _overViewCommand;
        public ICommand overViewCommand {
            get {
                return _overViewCommand;
            }
            set {
                if (_overViewCommand != value) {
                    _overViewCommand = value;
                    this.onPropertyChanged(nameof(overViewCommand));
                }
            }
        }

        private ICommand _rideListCommand;
        public ICommand rideListCommand {
            get {
                return _rideListCommand;
            }
            set {
                if (_rideListCommand != value) {
                    _rideListCommand = value;
                    this.onPropertyChanged(nameof(rideListCommand));
                }
            }
        }

        private ICommand _profileCommand;
        public ICommand profileCommand {
            get {
                return _profileCommand;
            }
            set {
                if (_profileCommand != value) {
                    _profileCommand = value;
                    this.onPropertyChanged(nameof(profileCommand));
                }
            }
        }

        private ICommand _aboutCommand;
        public ICommand aboutCommand {
            get {
                return _aboutCommand;
            }
            set {
                if (_aboutCommand != value) {
                    _aboutCommand = value;
                    this.onPropertyChanged(nameof(aboutCommand));
                }
            }
        }

        private ICommand _logoutCommand;
        public ICommand logoutCommand {
            get {
                return _logoutCommand;
            }
            set {
                if (_logoutCommand != value) {
                    _logoutCommand = value;
                    this.onPropertyChanged(nameof(logoutCommand));
                }
            }
        }

        public MainMasterViewModel(Page detailPage)
        {
            var mainNavigation = detailPage.Navigation;

            overViewCommand = new Command(async o => {
                await replacePage(mainNavigation, new OverViewMapPage());
            });
            rideListCommand = new Command(async o => {
                await replacePage(mainNavigation, new RideListPage());
            });
            profileCommand = new Command(async o => {
                System.Diagnostics.Debug.WriteLine("Profile!");
            });
            aboutCommand = new Command(async o => {
                System.Diagnostics.Debug.WriteLine("About!");
            });
            logoutCommand = new Command(async o => {
                System.Diagnostics.Debug.WriteLine("Logout!");
            });
        }

        private async Task replacePage(INavigation mainNavigation, Page newPage)
        {
            mainNavigation.InsertPageBefore(newPage, mainNavigation.NavigationStack[0]);
            isSideMenuPresented = !isSideMenuPresented;
            await mainNavigation.PopToRootAsync();
        }
    }
}
