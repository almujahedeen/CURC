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

[assembly: ExportEffect(typeof(EditorEffect), nameof(EditorEffect))]
namespace Curc.Droid.Effects
{
    class EditorEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try {
                var editText = Control as EditText;
                editText.SetSelectAllOnFocus(true);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}