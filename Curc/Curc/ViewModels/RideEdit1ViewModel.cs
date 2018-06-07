using Curc.Models;
using Curc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.ViewModels
{
    public class RideEdit1ViewModel : BaseModel
    {
        private ICommand _editRouteCommand;
        public ICommand editRouteCommand {
            get {
                return _editRouteCommand;
            }
            set {
                if (_editRouteCommand != value) {
                    _editRouteCommand = value;
                    this.onPropertyChanged(nameof(editRouteCommand));
                }
            }
        }

        public RideEdit1ViewModel(INavigation mainNavigation, RideItemModelFull editableModel)
        {
            editRouteCommand = new Command(async i => {
                editableModel.rideDate = new DateTime(
                    editableModel.rideDate.Year,
                    editableModel.rideDate.Month,
                    editableModel.rideDate.Day,
                    editableModel.rideTime.Hours,
                    editableModel.rideTime.Minutes,
                    editableModel.rideTime.Seconds);
                editableModel.rideEdit2ViewModel = new RideEdit2ViewModel(mainNavigation, editableModel);

                var edit2 = new RideEdit2Page();
                edit2.BindingContext = editableModel.rideEdit2ViewModel;
                await mainNavigation?.PushAsync(edit2);
            });
        }
    }
}
