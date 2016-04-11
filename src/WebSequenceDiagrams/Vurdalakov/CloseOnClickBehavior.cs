namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <Button vurdalakov:CloseOnClickBehavior />
    public class CloseOnClickBehavior
    {
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(Boolean), typeof(CloseOnClickBehavior), new PropertyMetadata(false, OnEnabledPropertyChanged));

        public static Boolean GetEnabled(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(EnabledProperty, value);
        }

        static void OnEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var button = dependencyObject as Button;
            if (null == button)
            {
                return;
            }

            if ((Boolean)args.NewValue)
            {
                button.Click += OnButtonClick;
            }
            else
            {
                button.Click -= OnButtonClick;
            }
        }

        static void OnButtonClick(Object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (null == button)
            {
                return;
            }

            var window = Window.GetWindow(button);
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
