namespace Vurdalakov
{
    using System;
    using System.Windows;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <Window
    //     vurdalakov:DialogResultBehavior.DialogResult="{Binding DialogResult}" />
    public static class DialogResultBehavior
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(Boolean?), typeof(DialogResultBehavior), new PropertyMetadata(null, OnDialogResultPropertyChanged));

        public static Boolean? GetDialogResult(DependencyObject dependencyObject)
        {
            return (Boolean?)dependencyObject.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject dependencyObject, Boolean? value)
        {
            dependencyObject.SetValue(DialogResultProperty, value);
        }

        private static void OnDialogResultPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var window = dependencyObject as Window;
            if (window != null)
            {
                window.DialogResult = e.NewValue as Boolean?;
            }
        }
    }
}
