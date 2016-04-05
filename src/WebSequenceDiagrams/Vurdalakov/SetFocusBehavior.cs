namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    // <TextBox Text="{Binding UserName}" vurdalakov:SetFocusBehavior.SetFocus="{Binding SetUserNameFocus}" />
    public static class SetFocusBehavior
    {
        public static readonly DependencyProperty SetFocusProperty =
            DependencyProperty.RegisterAttached("SetFocus", typeof(Boolean), typeof(SetFocusBehavior), new PropertyMetadata(false, OnSetFocusPropertyChanged));

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
            var control = dependencyObject as Control;
            if ((null == control) || !(e.NewValue is Boolean))
            {
                return;
            }

            if ((Boolean)e.NewValue)
            {
                control.Focus();
            }
        }
    }
}
