using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Curc.Behaviors
{
    public class CircularButtonBehavior : BehaviorBase<Button>
    {
        public static readonly BindableProperty attachBehaviorProperty =
            BindableProperty.CreateAttached(
                "attachBehavior",
                typeof(bool),
                typeof(CircularButtonBehavior),
                default(bool), //Don't attach at first set to false
                propertyChanged: attachChanged);

        public static bool GetattachBehavior(BindableObject view)
        {
            return (bool)view.GetValue(attachBehaviorProperty);
        }
        public static void SetattachBehavior(BindableObject view, bool value)
        {
            view.SetValue(attachBehaviorProperty, value);
        }

        private static void attachChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = bindable as Button;
            var isAttached = (bool)newValue;
            if (isAttached)
                button.Behaviors.Add(new CircularButtonBehavior());
            else
                button.Behaviors.Clear();
        }

        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SizeChanged += Bindable_SizeChanged;
        }

        protected override void OnDetachingFrom(Button bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SizeChanged -= Bindable_SizeChanged;
        }

        private void Bindable_SizeChanged(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.CornerRadius = (int)button.Height/2;
        }
    }
}
