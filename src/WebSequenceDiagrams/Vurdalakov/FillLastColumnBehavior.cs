namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class FillLastColumnBehavior
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(Boolean), typeof(FillLastColumnBehavior), new UIPropertyMetadata(false, OnAttachChanged));

        public static Boolean GetAttach(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(AttachProperty, value);
        }

        private static void OnAttachChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null)
            {
                var attach = (Boolean)e.NewValue;

                if (attach)
                {
                    listView.SizeChanged += OnListViewSizeChanged;
                }
                else
                {
                    listView.SizeChanged -= OnListViewSizeChanged;
                }
            }
        }

        private static void OnListViewSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null)
            {
                var gridView = listView.View as GridView;

                if (gridView != null)
                {
                    if (Double.IsNaN(listView.ActualWidth))
                    {
                        listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    }

                    var width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth - 10;
                    var lastColumn = gridView.Columns.Count - 1;

                    for (int i = 0; i < lastColumn; i++)
                    {
                        width -= gridView.Columns[i].ActualWidth;
                    }

                    gridView.Columns[lastColumn].Width = width >= 0 ? width : 0;
                }
            }
        }
    }
}
