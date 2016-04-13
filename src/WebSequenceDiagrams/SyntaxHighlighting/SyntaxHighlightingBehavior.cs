namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    using ICSharpCode.AvalonEdit;

    // xmlns:vurdalakov="clr-namespace:Vurdalakov"
    // <vurdalakov:AvalonEditor vurdalakov:SyntaxHighlightingBehavior.Enabled="True" />
    // <icsharpcode:TextEditor vurdalakov:SyntaxHighlightingBehavior.Enabled="True" />
    public class SyntaxHighlightingBehavior
    {
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(SyntaxHighlightingBehavior), new PropertyMetadata(false, OnPropertyChanged));

        public static Boolean GetEnabled(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(EnabledProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var textEditor = dependencyObject as TextEditor;

            if (textEditor != null)
            {
                if ((Boolean)e.NewValue)
                {
                    textEditor.TextArea.TextView.LineTransformers.Add(new ParticipantsColorizer(textEditor));
                }
                else
                {
                    textEditor.TextArea.TextView.LineTransformers.Clear();
                }
            }
        }
    }
}
