using Curc.Helpers;
using Curc.Models;
using Curc.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Curc.Views
{
    public class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            Master = new MainMasterPage();
            Detail = new NavigationPage(new OverViewMapPage());
            this.BindingContext = new MainMasterViewModel(this.Detail);
            Master.BindingContext = this.BindingContext;
            this.SetBinding(MasterDetailPage.IsPresentedProperty, new Binding("isSideMenuPresented", BindingMode.TwoWay)); // Really? default binding mode 1 way? T_T
            Cache.instance.loginModel = new LoginModel {
                userImage = "profile_pic",
                userName = "Al-Mujahedeen",
                isAdmin = true
            };
        }
    }
}
