namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using System.Xml;

    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;

    public class AvalonEditor : TextEditor, INotifyPropertyChanged
    {
        public AvalonEditor()
        {
            this.TextArea.Caret.PositionChanged += OnCaretPositionChanged;
            this.TextChanged += OnTextChanged;

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            if (FocusOnLoad)
            {
                this.Focus();
            }
        }

        // FocusOnLoad

        public Boolean FocusOnLoad { get; set; }

        // CaretOffset

        public static DependencyProperty CaretOffsetProperty =
            DependencyProperty.Register("CaretOffset", typeof(Int32), typeof(AvalonEditor), new UIPropertyMetadata(0, OnCaretOffsetPropertyChanged));

        public new Int32 CaretOffset
        {
            get { return (Int32)GetValue(CaretOffsetProperty); }
            set { SetValue(CaretOffsetProperty, value); base.CaretOffset = value;  }
        }

        private static void OnCaretOffsetPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var avalonEditor = dependencyObject as AvalonEditor;

            avalonEditor.CurrentLine = avalonEditor.TextArea.Caret.Line;
            avalonEditor.OnPropertyChanged(() => avalonEditor.CurrentLine);

            avalonEditor.CurrentColumn = avalonEditor.TextArea.Caret.Column;
            avalonEditor.OnPropertyChanged(() => avalonEditor.CurrentColumn);
        }

        private void OnCaretPositionChanged(Object sender, EventArgs e)
        {
            this.CaretOffset = base.CaretOffset;
            this.OnPropertyChanged(() => this.CaretOffset);
        }

        // CurrentLine

        public static DependencyProperty CurrentLineProperty =
            DependencyProperty.Register("CurrentLine", typeof(Int32), typeof(AvalonEditor), new UIPropertyMetadata(0, OnCurrentLinePropertyChanged));

        public Int32 CurrentLine
        {
            get { return (Int32)GetValue(CurrentLineProperty); }
            set { SetValue(CurrentLineProperty, value);  }
        }

        private static void OnCurrentLinePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var avalonEditor = dependencyObject as AvalonEditor;

            if (avalonEditor.TextArea.Caret.Line != (Int32)e.NewValue)
            {
                avalonEditor.TextArea.Caret.Line = (Int32)e.NewValue;
            }
        }

        // CurrentColumn

        public static DependencyProperty CurrentColumnProperty =
            DependencyProperty.Register("CurrentColumn", typeof(Int32), typeof(AvalonEditor), new UIPropertyMetadata(0, OnCurrentColumnPropertyChanged));

        public Int32 CurrentColumn
        {
            get { return (Int32)GetValue(CurrentColumnProperty); }
            set { SetValue(CurrentColumnProperty, value); }
        }

        private static void OnCurrentColumnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var avalonEditor = dependencyObject as AvalonEditor;

            if (avalonEditor.TextArea.Caret.Column != (Int32)e.NewValue)
            {
                avalonEditor.TextArea.Caret.Column = (Int32)e.NewValue;
            }
        }

        // Text

        public static DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(AvalonEditor), new UIPropertyMetadata("", OnTextPropertyChanged));

        public new String Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private static void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var avalonEditor = dependencyObject as AvalonEditor;

            if (avalonEditor.Text != e.NewValue as String)
            {
                avalonEditor.Text = e.NewValue as String;
            }
        }

        private void OnTextChanged(Object sender, EventArgs e)
        {
            SetValue(TextProperty, base.Text);
            this.OnPropertyChanged(() => this.Text);
        }

        // SyntaxHighlightingFromResource

        public static DependencyProperty SyntaxHighlightingFromResourceProperty =
            DependencyProperty.Register("SyntaxHighlightingFromResource", typeof(String), typeof(AvalonEditor), new UIPropertyMetadata("", OnSyntaxHighlightingFromResourcePropertyChanged));

        public String SyntaxHighlightingFromResource
        {
            get { return GetValue(SyntaxHighlightingFromResourceProperty) as String; }
            set { SetValue(SyntaxHighlightingFromResourceProperty, value); }
        }

        private static void OnSyntaxHighlightingFromResourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(e.NewValue as String))
            {
                using (var xmlTextReader = new XmlTextReader(stream))
                {
                    (dependencyObject as AvalonEditor).SyntaxHighlighting = HighlightingLoader.Load(xmlTextReader, HighlightingManager.Instance);
                }
            }
        }

        #region INotifyPropertyChanged

        // INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(this.GetPropertyName(propertyExpresssion));
                this.InvokeIfRequired(() => this.PropertyChanged(this, e));
            }
        }

        private String GetPropertyName<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (null == propertyExpresssion)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            MemberExpression memberExpression = propertyExpresssion.Body as MemberExpression;
            if (null == memberExpression)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            PropertyInfo property = memberExpression.Member as PropertyInfo;
            if (null == property)
            {
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            }

            MethodInfo methodInfo = property.GetGetMethod(true);
            if (methodInfo.IsStatic)
            {
                throw new ArgumentException("The referenced property is a static property.", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        // InvokeIfRequired

        private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        protected void InvokeIfRequired(Action action)
        {
            if (this.dispatcher.Thread != Thread.CurrentThread)
            {
                this.dispatcher.Invoke(DispatcherPriority.DataBind, action);
            }
            else
            {
                action();
            }
        }

        #endregion
    }
}
