namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <PasswordBox vurdalakov:PasswordBoxBindingBehavior.Enabled="True"  vurdalakov:PasswordBoxBindingBehavior.Password="{Binding Password}" />
    public class PasswordBoxBindingBehavior
    {
        // Enabled

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(Boolean), typeof(PasswordBoxBindingBehavior), new PropertyMetadata(false, OnEnabledPropertyChanged));

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
            var passwordBox = dependencyObject as PasswordBox;
            if (null == passwordBox)
            {
                return;
            }

            if ((Boolean)args.NewValue)
            {
                passwordBox.PasswordChanged += OnPasswordBoxPasswordChanged;
            }
            else
            {
                passwordBox.PasswordChanged -= OnPasswordBoxPasswordChanged;
            }
        }

        private static void OnPasswordBoxPasswordChanged(Object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (null == passwordBox)
            {
                return;
            }

            SetPassword(sender as DependencyObject, passwordBox.Password);
        }

        // Password

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(String), typeof(PasswordBoxBindingBehavior), new PropertyMetadata(null, OnPasswordPropertyChanged));

        public static String GetPassword(DependencyObject dependencyObject)
        {
            return (String)dependencyObject.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dependencyObject, String value)
        {
            dependencyObject.SetValue(PasswordProperty, value);
        }

        static void OnPasswordPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var passwordBox = dependencyObject as PasswordBox;
            if (null == passwordBox)
            {
                return;
            }

            passwordBox.Password = args.NewValue as String;
        }
    }
}
