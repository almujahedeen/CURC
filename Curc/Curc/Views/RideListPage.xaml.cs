﻿using Curc.ViewModels;
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
	public partial class RideListPage : ContentPage
	{
		public RideListPage ()
		{
            this.BindingContext = new RideListViewModel(this);
			InitializeComponent ();
		}
	}
}