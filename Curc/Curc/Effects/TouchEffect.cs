using Curc.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Curc.Effects
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("Effects.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public virtual void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
            System.Diagnostics.Debug.WriteLine($"\nTouch Event: {args.Type}; Is in Contact: {args.IsInContact}; Location: {args.Location}\n");
        }
    }

    public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }

        public long Id { private set; get; }

        public TouchActionType Type { private set; get; }

        public Point Location { private set; get; }

        public bool IsInContact { private set; get; }
    }
}
