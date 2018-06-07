using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Curc.Behaviors
{
    public class ListViewNoHighlightCommandBehavior : BehaviorBase<ListView>
    {
        private ListView listView;

        public static readonly BindableProperty itemSelectedCommandProperty =
            BindableProperty.Create(
                nameof(itemSelectedCommand),
                typeof(ICommand),
                typeof(ListViewNoHighlightCommandBehavior),
                default(ICommand),
                BindingMode.TwoWay,
                propertyChanged: (bindableObject, oV, nV) => {
                    var owner = bindableObject as ListViewNoHighlightCommandBehavior;
                    var newValue = (ICommand)nV;
                });
        public ICommand itemSelectedCommand {
            get { return (ICommand)GetValue(itemSelectedCommandProperty); }
            set { SetValue(itemSelectedCommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            listView = bindable;
            bindable.ItemSelected += Bindable_ItemSelected;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemSelected -= Bindable_ItemSelected;
        }

        private void Bindable_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
                itemSelectedCommand?.Execute(e.SelectedItem);
            listView.SelectedItem = null; //The key on Android to completely remove highlight along with the listview effect
        }
    }
}
