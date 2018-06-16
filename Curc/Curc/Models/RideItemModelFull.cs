using Curc.Models;
using Curc.ViewModels;
using Curc.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Models
{
    public class RideItemModelFull : RideItemModel
    {
        public string rideDescription { get; set; }
        public TimeSpan rideTime { get; set; }

        //Map properties
        public ObservableCollection<UserPinModel> pins { get; set; }
        public ObservableCollection<Polyline> routes { get; set; }

        //ViewModels
        public MapViewModel mapViewModel { get; set; }
        public RideEdit1ViewModel rideEdit1ViewModel { get; set; }
        public RideEdit2ViewModel rideEdit2ViewModel { get; set; }


        //public INavigation mainNavigation { get; set; }

        //private ICommand _editRouteCommand;
        //public ICommand editRouteCommand {
        //    get {
        //        return _editRouteCommand;
        //    }
        //    set {
        //        if (_editRouteCommand != value) {
        //            _editRouteCommand = value;
        //            this.onPropertyChanged(nameof(editRouteCommand));
        //        }
        //    }
        //}

        //public RideItemModelFull()
        //{
        //    init();
        //    editRouteCommand = new Command(async o => {
        //        var editableItem = new EditableMapViewModel {
        //            rideName = this.rideName,
        //            rideDescription = this.rideDescription,
        //            rideDate = this.rideDate,
        //            rideTime = this.rideDate.TimeOfDay,
        //            rideStartLocation = this.rideStartLocation,
        //            rideEndLocation = this.rideEndLocation,
        //            mainNavigation = this.mainNavigation
        //        };

        //        var edit1 = new RideEdit1Page();
        //        edit1.BindingContext = editableItem;
        //        await mainNavigation?.PushAsync(edit1);
        //    });
        //}

        //private async Task init()
        //{
        //    rideDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at dictum lorem. Suspendisse et felis et nunc rhoncus condimentum. In non nunc non arcu sodales accumsan. Mauris et tempus lectus. Phasellus sagittis massa non posuere varius. Aliquam ut fringilla mauris. Integer interdum, leo eu posuere faucibus, ipsum est tempus tortor, et ornare neque odio a justo. Donec cursus mattis auctor. Phasellus id mi id felis interdum pellentesque. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla id purus nec erat viverra fermentum. Sed in neque nisl.";
        //}
    }
}
