namespace Vurdalakov
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <TextBox vurdalakov:TextBoxLineColumnBehavior.LineNumber="{Binding LineNumber, Mode=TwoWay}" vurdalakov:TextBoxLineColumnBehavior.ColumnNumber="{Binding ColumnNumber, Mode=TwoWay}" vurdalakov:TextBoxLineColumnBehavior.Attach="True" />
    public class TextBoxLineColumnBehavior : DependencyObject
    {
        // LineNumber

        public static readonly DependencyProperty LineNumberProperty =
            DependencyProperty.RegisterAttached("LineNumber", typeof(Int32), typeof(TextBoxLineColumnBehavior), new UIPropertyMetadata(1, OnLineNumberChanged));

        public static Int32 GetLineNumber(DependencyObject dependencyObject)
        {
            return (Int32)dependencyObject.GetValue(LineNumberProperty);
        }

        public static void SetLineNumber(DependencyObject dependencyObject, Int32 value)
        {
            dependencyObject.SetValue(LineNumberProperty, value);
        }

        private static void OnLineNumberChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var textBox = dependencyObject as TextBox;

            if (textBox != null)
            {
                textBox.CaretIndex = textBox.GetCharacterIndexFromLineIndex(GetLineNumber(dependencyObject) - 1);
            }
        }

        // ColumnNumber

        public static readonly DependencyProperty ColumnNumberProperty =
            DependencyProperty.RegisterAttached("ColumnNumber", typeof(Int32), typeof(TextBoxLineColumnBehavior), new UIPropertyMetadata(1, OnColumnNumberChanged));

        public static Int32 GetColumnNumber(DependencyObject dependencyObject)
        {
            return (Int32)dependencyObject.GetValue(ColumnNumberProperty);
        }

        public static void SetColumnNumber(DependencyObject dependencyObject, Int32 value)
        {
            dependencyObject.SetValue(ColumnNumberProperty, value);
        }

        private static void OnColumnNumberChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var textBox = dependencyObject as TextBox;

            if (textBox != null)
            {
                textBox.CaretIndex = textBox.GetCharacterIndexFromLineIndex(GetLineNumber(dependencyObject) - 1) + GetColumnNumber(dependencyObject) - 1;
            }
        }

        // Attach

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(Boolean), typeof(TextBoxLineColumnBehavior), new UIPropertyMetadata(false, OnAttachChanged));

        public static Boolean GetAttach(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(AttachProperty, value);
        }

        private static void OnAttachChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var textBox = dependencyObject as TextBox;

            if (textBox != null)
            {
                var attach = (Boolean)e.NewValue;

                if (attach)
                {
                    textBox.SelectionChanged += OnTextBoxSelectionChanged;
                }
                else
                {
                    textBox.SelectionChanged -= OnTextBoxSelectionChanged;
                }
            }
        }

        private static void OnTextBoxSelectionChanged(Object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var dependencyObject = sender as DependencyObject;

            if ((textBox != null) && (dependencyObject != null))
            {
                var pos = textBox.CaretIndex;
                var line = textBox.GetLineIndexFromCharacterIndex(pos);
                SetLineNumber(dependencyObject, line + 1);
                SetColumnNumber(dependencyObject, pos - textBox.GetCharacterIndexFromLineIndex(line) + 1);
            }
        }
    }
}
