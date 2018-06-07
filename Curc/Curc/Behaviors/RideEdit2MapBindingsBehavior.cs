using Curc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Curc.Behaviors
{
    public class RideEdit2MapBindingsBehavior : BehaviorBase<Map>
    {
        private Map map;

        public static readonly BindableProperty visibleRegionProperty =
            BindableProperty.Create(
                nameof(visibleRegion),
                typeof(MapSpan),
                typeof(RideEdit2MapBindingsBehavior),
                default(MapSpan),
                BindingMode.OneWayToSource,
                propertyChanged: (bindableObject, oV, nV) => {
                    var owner = bindableObject as RideEdit2MapBindingsBehavior;
                    var newValue = (MapSpan)nV;
                });
        public MapSpan visibleRegion {
            get { return (MapSpan)GetValue(visibleRegionProperty); }
            set { SetValue(visibleRegionProperty, value); }
        }

        public static readonly BindableProperty moveToRegionCommandProperty =
           BindableProperty.Create(
               nameof(moveToRegionCommand),
               typeof(ICommand),
               typeof(RideEdit2MapBindingsBehavior),
               default(ICommand),
               BindingMode.OneWayToSource,
               propertyChanged: (bindableObject, oV, nV) => {
                   var owner = bindableObject as RideEdit2MapBindingsBehavior;
                   var newValue = (ICommand)nV;
               });
        public ICommand moveToRegionCommand {
            get { return (ICommand)GetValue(moveToRegionCommandProperty); }
            set { SetValue(moveToRegionCommandProperty, value); }
        }

        public static readonly BindableProperty pinsProperty =
            BindableProperty.Create(
                nameof(pins),
                typeof(ObservableCollection<UserPinModel>),
                typeof(RideEdit2MapBindingsBehavior),
                default(ObservableCollection<UserPinModel>),
                BindingMode.TwoWay,
                propertyChanged: (bindableObject, oV, nV) => {
                    var owner = bindableObject as RideEdit2MapBindingsBehavior;
                    var newValue = (ObservableCollection<UserPinModel>)nV;
                    if (oV != null) {
                        var oldValue = (ObservableCollection<UserPinModel>)oV;
                        owner.map.Pins.Clear();
						oldValue.CollectionChanged -= owner.pinsChanged;
                    }
                    foreach (var item in newValue)
                        owner.map.Pins.Add(item.pin);
                    newValue.CollectionChanged += owner.pinsChanged;
                });

        public ObservableCollection<UserPinModel> pins {
            get { return (ObservableCollection<UserPinModel>)GetValue(pinsProperty); }
            set { SetValue(pinsProperty, value); }
        }

		public static readonly BindableProperty routesProperty = BindableProperty.Create(
			nameof(routes),
			typeof(ObservableCollection<Polyline>),
			typeof(RideEdit2MapBindingsBehavior),
			default(ObservableCollection<Polyline>),
			BindingMode.TwoWay,
			propertyChanged: (bindable, p1, p2) => {
				var view = bindable as RideEdit2MapBindingsBehavior;
				var newValue = (ObservableCollection<Polyline>)p2;
				if (p1 != null) {
					var oldValue = (ObservableCollection<Polyline>)p1;
					view.map.Polylines.Clear();
					oldValue.CollectionChanged -= view.PolyLinesChanged;
				}
				foreach (var item in newValue)
					view.map.Polylines.Add(item);
				newValue.CollectionChanged += view.PolyLinesChanged;
			}
		);
		public ObservableCollection<Polyline> routes {
			get { return (ObservableCollection<Polyline>)GetValue(routesProperty); }
			set { SetValue(routesProperty, value); }
		}

		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BindingContext!=null) {
                //Init
                moveToRegionCommand = new Command<MapSpan>(mapSpan => map.MoveToRegion(mapSpan));
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
            this.visibleRegion = map.VisibleRegion;
        }

        private void pinsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action) {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (UserPinModel item in e.NewItems)
                        map.Pins.Add(item.pin);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (UserPinModel item in e.OldItems)
                        map.Pins.Remove(item.pin);
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    map.Pins.Clear();
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();
            }
        }

		void PolyLinesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
				case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					foreach (Polyline item in e.NewItems)
						map.Polylines.Add(item);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
					foreach (Polyline item in e.OldItems)
						map.Polylines.Remove(item);
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
					map.Polylines.Clear();
					break;

				case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
					throw new NotImplementedException();
				case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
					throw new NotImplementedException();
			}
		}
    }
}
