using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Curc.Helpers
{
    public class ScaleResizer
    {
        public static Element scaleChild(Element child)
        {
            var label = child as Label;
            var button = child as Button;
            var entry = child as Entry;
            var editor = child as Editor;
            var listView = child as ListView;
            var tableView = child as TableView;

            var stackLayout = child as StackLayout;
            var flexLayout = child as FlexLayout;
            var gridLayout = child as Grid;
            var relativeLayout = child as RelativeLayout;
            var absoluteLayout = child as AbsoluteLayout;
            var contentView = child as ContentView;
            var frame = child as Frame;
            var scrollView = child as ScrollView;
            var cell = child as Cell;

            var visualElement = child as VisualElement;
            var view = child as View;
            var layout = child as Layout;

            if (visualElement != null) {
                visualElement.WidthRequest = visualElement.WidthRequest < 0 ? visualElement.WidthRequest : visualElement.WidthRequest * Constants.scale;
                visualElement.HeightRequest = visualElement.HeightRequest < 0 ? visualElement.HeightRequest : visualElement.HeightRequest * Constants.scale;

                visualElement.MinimumWidthRequest = visualElement.MinimumWidthRequest < 0 ? visualElement.MinimumWidthRequest : visualElement.MinimumWidthRequest * Constants.scale;
                visualElement.MinimumHeightRequest = visualElement.MinimumHeightRequest < 0 ? visualElement.MinimumHeightRequest : visualElement.MinimumHeightRequest * Constants.scale;
            }

            if (view != null) {
                view.Margin = new Thickness(view.Margin.Left * Constants.scale,
                                               view.Margin.Top * Constants.scale,
                                               view.Margin.Right * Constants.scale,
                                            view.Margin.Bottom * Constants.scale);

            }

            if (layout != null) {
                layout.Padding = new Thickness(layout.Padding.Left * Constants.scale,
                                               layout.Padding.Top * Constants.scale,
                                               layout.Padding.Right * Constants.scale,
                                               layout.Padding.Bottom * Constants.scale);
            }

            // View type selection
            if (contentView != null) {
                if (frame != null) {
                    frame.CornerRadius = (float)(frame.CornerRadius * Constants.scale);
                }
            } else if (scrollView != null) {

            } else if (stackLayout != null) {
                stackLayout.Spacing = stackLayout.Spacing * Constants.scale;
            } else if (flexLayout != null) {
                //Unused
            } else if (gridLayout != null) {
                gridLayout.RowSpacing = gridLayout.RowSpacing * Constants.scale;
                gridLayout.ColumnSpacing = gridLayout.ColumnSpacing * Constants.scale;
            } else if (relativeLayout != null) {
                //Unused
            } else if (absoluteLayout != null) {
                //Unused
            } else if (label != null) {
                label.FontSize = label.FontSize * Constants.scale;
            } else if (button != null) {
                button.BorderWidth = button.BorderWidth < 0 ? button.BorderWidth : button.BorderWidth * Constants.scale;
                button.BorderRadius = (int)(button.BorderRadius * Constants.scale);
                button.FontSize = button.FontSize * Constants.scale;
            } else if (entry != null) {
                entry.FontSize = entry.FontSize * Constants.scale;
            } else if (editor != null) {
                entry.FontSize = editor.FontSize * Constants.scale;
            } else if (listView != null) {
                //TODO: Reach bottom event for Android and iOS.
                listView.RowHeight = (int)(listView.RowHeight * Constants.scale);
            } else if (tableView != null) {
                tableView.RowHeight = (int)(tableView.RowHeight * Constants.scale);
            } else if (cell != null) {
                //TODO: No hightlight iOS only.
            }

            return child;
        }
    }
}
