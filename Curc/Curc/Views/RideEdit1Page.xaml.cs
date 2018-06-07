using Curc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Curc.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RideEdit1Page : ContentPage
	{
		public RideEdit1Page ()
		{
            this.BindingContext = new RideItemModel();
			InitializeComponent ();
		}
	}
}