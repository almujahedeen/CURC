using Curc.Extensions;
using Curc.Helpers;
using Curc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Behaviors
{
    public sealed class OverviewMapBindingsBehavior : BehaviorBase<Map>
    {
        private Map map;

        public static readonly BindableProperty visibleRegionProperty =
            BindableProperty.Create(
                nameof(visibleRegion),
                typeof(MapSpan),
                typeof(OverviewMapBindingsBehavior),
                default(MapSpan),
                BindingMode.OneWayToSource);
        public MapSpan visibleRegion {
            get { return (MapSpan)GetValue(visibleRegionProperty); }
            set { SetValue(visibleRegionProperty, value); }
        }

        public static readonly BindableProperty moveToRegionCommandProperty =
            BindableProperty.Create(
                nameof(moveToRegionCommand),
                typeof(Command<MapSpan>),
                typeof(OverviewMapBindingsBehavior),
                default(Command<MapSpan>),
                BindingMode.OneWayToSource);
        public Command<MapSpan> moveToRegionCommand {
            get { return (Command<MapSpan>)GetValue(moveToRegionCommandProperty); }
            set { SetValue(moveToRegionCommandProperty, value); }
        }

        public static readonly BindableProperty userPinsProperty =
            BindableProperty.Create(
                nameof(userPins),
                typeof(ObservableCollection<UserPinModel>),
                typeof(OverviewMapBindingsBehavior),
                default(ObservableCollection<UserPinModel>),
                BindingMode.TwoWay,
                propertyChanged: (bindableObject, oV, nV) => {
                    var owner = bindableObject as OverviewMapBindingsBehavior;
                    var newValue = (ObservableCollection<UserPinModel>)nV;
                    foreach (var item in newValue)
                        owner.map.Pins.Add(item.pin);

                    newValue.CollectionChanged += owner.NewValue_CollectionChanged;
                    if (oV != null) {
                        var oldValue = (ObservableCollection<Pin>)oV;
                        owner.map.Pins.Clear();
                        oldValue.CollectionChanged -= owner.NewValue_CollectionChanged;
                    }
                });
        public ObservableCollection<UserPinModel> userPins {
            get { return (ObservableCollection<UserPinModel>)GetValue(userPinsProperty); }
            set { SetValue(userPinsProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BindingContext != null) {
                //Init
                moveToRegionCommand = new Command<MapSpan>(ms => map.MoveToRegion(ms, true));
            }
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            map = bindable;
            map.CameraIdled += Map_CameraIdled;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            map.CameraIdled -= Map_CameraIdled;
        }

        private void Map_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            visibleRegion = map.VisibleRegion;
        }

        public void NewValue_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (UserPinModel item in e.NewItems) {
                        map.Pins.Add(item.pin);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (UserPinModel item in e.OldItems) {
                        map.Pins.Remove(item.pin);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    map.Pins.Clear();
                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
            }
        }
    }
}
