namespace Vurdalakov
{
    using System;
    using System.Windows;

    public partial class AboutWindow : Window
    {
        public AboutWindow(Window owner)
        {
            InitializeComponent();

            this.Loaded += (s, e) => { this.DataContext = new ViewModelBase(); };

            this.Owner = owner;

            this.SetDialogStyle();
        }
    }
}
