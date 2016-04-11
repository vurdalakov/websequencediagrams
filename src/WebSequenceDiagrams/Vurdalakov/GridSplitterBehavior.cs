namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <Grid vurdalakov:GridSplitterBehavior.SaveAndRestore="True" />
    public static class GridSplitterBehavior
    {
        // SaveAndRestore

        public static readonly DependencyProperty SaveAndRestoreProperty =
            DependencyProperty.RegisterAttached("SaveAndRestore", typeof(String), typeof(GridSplitterBehavior), new PropertyMetadata(null, OnSaveAndRestorePropertyChanged));

        public static String GetSaveAndRestore(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(SaveAndRestoreProperty) as String;
        }

        public static void SetSaveAndRestore(DependencyObject dependencyObject, String value)
        {
            dependencyObject.SetValue(SaveAndRestoreProperty, value);
        }

        private static void OnSaveAndRestorePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var grid = dependencyObject as Grid;
            if (null == grid)
            {
                return;
            }

            var window = grid.Parent as Window;

            if (window != null)
            {
                window.Loaded += (s, e1) => OnWindowLoaded(s, e1);
                window.Closing += (s, e1) => e1.Cancel = OnWindowClosing(s, e1);
            }
        }

        private static void OnWindowLoaded(Object s, RoutedEventArgs e)
        {
            var grid = FindGrid(s as Window);
            var name = GetSaveAndRestore(grid as DependencyObject);
            if ((null == grid) || (null == name))
            {
                return;
            }

            for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var column = grid.ColumnDefinitions[i];
                var width = PermanentSettings.Instance.Get(GetColumnName(name, i) + ".Width", -1.0);
                if (width >= 0)
                {
                    var type = PermanentSettings.Instance.Get<GridUnitType>(GetColumnName(name, i) + ".Type", GridUnitType.Auto);
                    column.Width = new GridLength(width, type);
                }
            }

            for (var i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var row = grid.RowDefinitions[i];
                var height = PermanentSettings.Instance.Get(GetRowName(name, i) + ".Height", -1.0);
                if (height >= 0)
                {
                    var type = PermanentSettings.Instance.Get<GridUnitType>(GetRowName(name, i) + ".Type", GridUnitType.Auto);
                    row.Height = new GridLength(height, type);
                }
            }
        }

        private static bool OnWindowClosing(Object s, CancelEventArgs e)
        {
            var grid = FindGrid(s as Window);
            var name = GetSaveAndRestore(grid as DependencyObject);
            if ((null == grid) || (null == name))
            {
                return false;
            }

            for (var i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var column = grid.ColumnDefinitions[i];
                PermanentSettings.Instance.Set(GetColumnName(name, i) + ".Width", column.ActualWidth);
                PermanentSettings.Instance.Set(GetColumnName(name, i) + ".Type", column.Width.GridUnitType);
            }

            for (var i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var row = grid.RowDefinitions[i];
                PermanentSettings.Instance.Set(GetRowName(name, i) + ".Height", row.ActualHeight);
                PermanentSettings.Instance.Set(GetRowName(name, i) + ".Type", row.Height.GridUnitType);
            }

            return false;
        }

        private static String GetColumnName(String name, Int32 index)
        {
            return String.Format("{0}.Column{1}", name, index);
        }

        private static String GetRowName(String name, Int32 index)
        {
            return String.Format("{0}.Row{1}", name, index);
        }

        private static Grid FindGrid(DependencyObject dependencyObject)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                if (child is Grid)
                {
                    return child as Grid;
                }
                else
                {
                    var grid = FindGrid(child);
                    if (grid != null)
                    {
                        return grid;
                    }
                }
            }

            return null;
        }
    }
}
