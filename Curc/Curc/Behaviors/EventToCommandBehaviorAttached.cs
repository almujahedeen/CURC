using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.Behaviors
{
    public static class EventToCommandBehaviorAttached
    {
        private static Delegate eventHandler;

        public static readonly BindableProperty eventNameProperty =
               BindableProperty.CreateAttached(
                   "eventName",
                   typeof(string),
                   typeof(EventToCommandBehaviorAttached),
                   default(string), //Don't attach at first set to false
                   propertyChanged: eventNamePropertyChanged);
        
        public static string GeteventName(BindableObject view)
        {
            return (string)view.GetValue(eventNameProperty);
        }
        public static void SeteventName(BindableObject view, string value)
        {
            view.SetValue(eventNameProperty, value);
        }

        public static readonly BindableProperty commandProperty =
            BindableProperty.CreateAttached(
                "command",
                typeof(ICommand),
                typeof(EventToCommandBehaviorAttached),
                default(ICommand));

        public static ICommand Getcommand(BindableObject view)
        {
            return (ICommand)view.GetValue(commandProperty);
        }
        public static void Setcommand(BindableObject view, ICommand value)
        {
            view.SetValue(commandProperty, value);
        }

        public static readonly BindableProperty commandParameterProperty =
            BindableProperty.CreateAttached(
                "commandParameter",
                typeof(string),
                typeof(EventToCommandBehaviorAttached),
                default(string), //Don't attach at first set to false
                propertyChanged: (bindableObject, oV, nV) => {
                    var newValue = (string)nV;
                });
        public static string GetcommandParameter(BindableObject view)
        {
            return (string)view.GetValue(commandParameterProperty);
        }
        public static void SetcommandParameter(BindableObject view, string value)
        {
            view.SetValue(commandParameterProperty, value);
        }

        private static void eventNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var newStringValue = newValue as string;
            var eventName = GeteventName(bindable);
            if (string.IsNullOrWhiteSpace(newStringValue))
                return;
            registerEvent(bindable, eventName);
        }

        private static void registerEvent(BindableObject bindable, string eventName)
        {
            EventInfo eventInfo = bindable.GetType().GetRuntimeEvent(eventName);
            if (eventInfo == null) {
                throw new ArgumentException(string.Format("EventToCommandBehaviorAttached: Can't register the '{0}' event.", eventName));
            }
            MethodInfo methodInfo = typeof(EventToCommandBehaviorAttached).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType);
            eventInfo.AddEventHandler(bindable, eventHandler);
        }

        private static void deRegisterEvent(BindableObject bindable, string name)
        {
            //TODO: Unused event cannot be registered for now.
        }

        private static void OnEvent(object sender, object eventArgs)
        {
            var command = Getcommand(sender as BindableObject);
            var commandParameter = GetcommandParameter(sender as BindableObject);
            if (commandParameter == null)
                command?.Execute(null);
            else
                command?.Execute(commandParameter);
        }
        
        private static void ViewCell_Tapped(object sender, EventArgs e)
        {
            Getcommand(sender as BindableObject).Execute(sender);
        }
    }
}
