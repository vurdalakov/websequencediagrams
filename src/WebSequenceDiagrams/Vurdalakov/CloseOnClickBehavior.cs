namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <Button vurdalakov:CloseOnClickBehavior />
    public class CloseOnClickBehavior
    {

        static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
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
                var dialogResult = GetDialogResult(sender as DependencyObject);
                if (dialogResult.HasValue)
                {
                    window.DialogResult = dialogResult;
                }

                window.Close();
            }
        }

        // Enabled

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(Boolean), typeof(CloseOnClickBehavior), new PropertyMetadata(false, OnPropertyChanged));

        public static Boolean GetEnabled(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(EnabledProperty, value);
        }

        // DialogResult

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(Boolean?), typeof(CloseOnClickBehavior), new PropertyMetadata(null, OnPropertyChanged));

        public static Boolean? GetDialogResult(DependencyObject dependencyObject)
        {
            return (Boolean?)dependencyObject.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject dependencyObject, Boolean? value)
        {
            dependencyObject.SetValue(DialogResultProperty, value);
        }
    }
}
