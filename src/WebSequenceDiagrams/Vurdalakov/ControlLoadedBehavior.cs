namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <TextBox vurdalakov:ControlLoadedBehavior.SetFocus="True" vurdalakov:ControlLoadedBehavior.SelectAll="True" />
    public static class ControlLoadedBehavior
    {
        // SetFocus

        public static readonly DependencyProperty SetFocusProperty =
            DependencyProperty.RegisterAttached("SetFocus", typeof(Boolean), typeof(FrameworkElement), new PropertyMetadata(false, OnSetFocusPropertyChanged));

        public static Boolean GetSetFocus(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(SetFocusProperty);
        }

        public static void SetSetFocus(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(SetFocusProperty, value);
        }

        private static void OnSetFocusPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control control = dependencyObject as Control;
            if ((null == control) || !(e.NewValue is Boolean))
            {
                return;
            }

            if ((Boolean)e.NewValue)
            {
                control.Loaded += (sender, args) => control.Focus();
            }
        }

        // SelectAll

        public static readonly DependencyProperty SelectAllProperty =
            DependencyProperty.RegisterAttached("SelectAll", typeof(Boolean), typeof(FrameworkElement), new PropertyMetadata(false, OnSelectAllPropertyChanged));

        public static Boolean GetSelectAll(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(SelectAllProperty);
        }

        public static void SetSelectAll(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(SelectAllProperty, value);
        }

        private static void OnSelectAllPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = dependencyObject as TextBox;
            if ((null == textBox) || !(e.NewValue is Boolean))
            {
                return;
            }

            if ((Boolean)e.NewValue)
            {
                textBox.Loaded += (sender, args) => textBox.SelectAll();
            }
        }
    }
}
