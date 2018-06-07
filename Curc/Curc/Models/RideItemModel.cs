using Curc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.Models
{
    public class RideItemModel : BaseModel
    {
        public string rideName { get; set; }
        public string rideStartLocation { get; set; }
        public string rideEndLocation { get; set; }
        public DateTime rideDate { get; set; }
    }
}
