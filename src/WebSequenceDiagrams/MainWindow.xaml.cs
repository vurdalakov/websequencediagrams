namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(this);
        }
    }
}
