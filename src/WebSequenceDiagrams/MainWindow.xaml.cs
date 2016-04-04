namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.Loaded += (s, e) => { this.DataContext = new MainViewModel(); };
            this.Loaded += OnMainWindowLoaded;
        }

        private MainViewModel _mainViewModel;
        private void OnMainWindowLoaded(Object sender, RoutedEventArgs e)
        {
            this._mainViewModel = new MainViewModel();

            this.DataContext = this._mainViewModel;
        }
    }
}
