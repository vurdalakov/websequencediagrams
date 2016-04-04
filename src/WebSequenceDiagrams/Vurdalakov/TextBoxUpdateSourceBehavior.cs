namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    // http://stackoverflow.com/a/4834720
    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <TextBox Text="{Binding TextProperty1, Mode=TwoWay}" vurdalakov:TextBoxUpdateSourceBehavior.UpdateSourceOnChange="True" />
    public class TextBoxUpdateSourceBehavior
    {
        public static bool GetUpdateSourceOnChange(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(UpdateSourceOnChangeProperty);
        }

        public static void SetUpdateSourceOnChange(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(UpdateSourceOnChangeProperty, value);
        }

        public static readonly DependencyProperty UpdateSourceOnChangeProperty =
            DependencyProperty.RegisterAttached("UpdateSourceOnChange", typeof(bool), typeof(TextBoxUpdateSourceBehavior), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = dependencyObject as TextBox;
            if (null == textBox)
            {
                return;
            }

            if ((Boolean)e.NewValue)
            {
                textBox.TextChanged += OnTextChanged;
            }
            else
            {
                textBox.TextChanged -= OnTextChanged;
            }
        }

        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (null == textBox)
            {
                return;
            }

            BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }
    }
}
