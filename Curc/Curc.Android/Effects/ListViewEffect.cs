using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Curc.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(ListViewEffect), nameof(ListViewEffect))]
namespace Curc.Droid.Effects
{
    public class ListViewEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try {
                var listView = Control as Android.Widget.ListView;
                listView.SetSelector(Resource.Layout.no_selector);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}