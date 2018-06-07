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
    public class MapViewModel : BaseModel
    {
        private RideItemModelFull model { get; set; }

        private ICommand _editCommand;
        public ICommand editCommand {
            get {
                return _editCommand;
            }
            set {
                if (_editCommand != value) {
                    _editCommand = value;
                    this.onPropertyChanged(nameof(editCommand));
                }
            }
        }
        public MapViewModel(INavigation mainNavigation, RideItemModelFull model)
        {
            this.model = model;
            init();
            editCommand = new Command(async o => {
                var editableItem = new RideItemModelFull {
                    rideName = model.rideName,
                    rideDescription = model.rideDescription,
                    rideDate = model.rideDate,
                    rideTime = model.rideDate.TimeOfDay,
                    rideStartLocation = model.rideStartLocation,
                    rideEndLocation = model.rideEndLocation
                };
                editableItem.rideEdit1ViewModel = new RideEdit1ViewModel(mainNavigation, editableItem);

                var edit1 = new RideEdit1Page();
                edit1.BindingContext = editableItem;
                await mainNavigation?.PushAsync(edit1);
            });
        }

        private async Task init()
        {
            model.rideDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at dictum lorem. Suspendisse et felis et nunc rhoncus condimentum. In non nunc non arcu sodales accumsan. Mauris et tempus lectus. Phasellus sagittis massa non posuere varius. Aliquam ut fringilla mauris. Integer interdum, leo eu posuere faucibus, ipsum est tempus tortor, et ornare neque odio a justo. Donec cursus mattis auctor. Phasellus id mi id felis interdum pellentesque. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla id purus nec erat viverra fermentum. Sed in neque nisl.";
        }
    }
}
