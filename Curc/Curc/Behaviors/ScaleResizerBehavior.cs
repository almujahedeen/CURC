using Curc.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Curc.Behaviors
{
    public static class ScaleResizerBehavior
    {
        public static readonly BindableProperty isAttachedProperty =
            BindableProperty.CreateAttached(
                "isAttached",
                typeof(bool),
                typeof(ScaleResizerBehavior),
                default(bool),
                BindingMode.TwoWay,
                propertyChanged: isAttachedChanged);

        public static bool GetisAttached(BindableObject view)
        {
            return (bool)view.GetValue(isAttachedProperty);
        }

        public static void SetisAttached(BindableObject view, bool value)
        {
            view.SetValue(isAttachedProperty, value);
        }

        private static void isAttachedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var element = bindable as Element;
            if (element != null) {
                if ((bool)newValue) {
                    element.ChildAdded += Element_ChildAdded;
                } else
                    element.ChildAdded -= Element_ChildAdded;
            }
        }

        static void Element_ChildAdded(object sender, ElementEventArgs e)
        {
            ScaleResizer.scaleChild(e.Element);
        }
    }
}
