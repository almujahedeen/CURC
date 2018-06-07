using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.Effects
{
    public class ButtonViewEffect : TouchEffect
    {
        public static readonly BindableProperty clickedCommandProperty =
            BindableProperty.CreateAttached(
                "clickedCommand",
                typeof(ICommand),
                typeof(ButtonViewEffect),
                default(ICommand), //Don't attach at first set to null
                propertyChanged: clickedCommandChanged);

        public static ICommand GetclickedCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(clickedCommandProperty);
        }
        public static void SetclickedCommand(BindableObject view, ICommand value)
        {
            view.SetValue(clickedCommandProperty, value);
        }

        private static void clickedCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null) {
                var command = newValue as ICommand;
                if (command != null)
                    view.Effects.Add(new ButtonViewEffect());
                else {
                    var toRemove = view.Effects.FirstOrDefault(e => e is ButtonViewEffect);
                    if (toRemove != null)
                        view.Effects.Remove(toRemove);
                }
            }
        }

        public override void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            base.OnTouchAction(element, args);

            var view = element as View;
            switch (args.Type) {
                case Enumerations.TouchActionType.Entered:
                    break;
                case Enumerations.TouchActionType.Pressed:
                    view.Opacity = 0.5;
                    break;
                case Enumerations.TouchActionType.Moved:
                    break;
                case Enumerations.TouchActionType.Released:
                    var command = GetclickedCommand(view);
					if (command != null && command.CanExecute(null))
						command.Execute(null);
                    view.Opacity = 1;
                    break;
                case Enumerations.TouchActionType.Exited:
                    view.Opacity = 1;
                    break;
                case Enumerations.TouchActionType.Cancelled:
                    break;
            }
        }
    }
}