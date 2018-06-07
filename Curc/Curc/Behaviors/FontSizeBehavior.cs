using Curc.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Curc.Behaviors
{
    public class FontSizeBehavior : Behavior<View>
    {
        public static readonly BindableProperty AttachBehaviorProperty =
            BindableProperty.CreateAttached(
                "AttachBehavior",
                typeof(int),
                typeof(FontSizeBehavior),
                -1, //Don't attach at first set to false
                propertyChanged: (bindableObject, oV, nV) => {
                    var newValue = (int)nV;
                    processNewValue(bindableObject, newValue);
                });
        public static int GetAttachBehavior(BindableObject view)
        {
            return (int)view.GetValue(AttachBehaviorProperty);
        }
        public static void SetAttachBehavior(BindableObject view, int value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }

        private static void processNewValue(BindableObject obj, int newValue)
        {
            if (newValue < 0)
                return;
            var entry = obj as Entry;
            var label = obj as Label;
            var button = obj as Button;
            var editor = obj as Editor;

            var newFontSize = newValue * Constants.scale;
            if (entry != null) {
                entry.FontSize = newFontSize;
            } else if (label != null) {
                label.FontSize = newFontSize;
            } else if (button != null) {
                button.FontSize = newFontSize;
            } else if (editor != null) {
                editor.FontSize = newFontSize;
            }
        }
        #region Unused - just to demo that you can use view.Behaviors.Add() and view.Event += or -= to unsubscribe
        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
        }
        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
        }
        #endregion
    }
}
