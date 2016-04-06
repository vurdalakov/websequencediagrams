namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TODO:
            //this._mainViewModel = new MainViewModel();
            //this.DataContext = this._mainViewModel;
            //this.Loaded += (s, e) => this._mainViewModel.OnMainWindowLoaded();

            this.Loaded += OnMainWindowLoaded;
            this.Closing += (s, e) => e.Cancel = this._mainViewModel.OnMainWindowClosing();
        }

        private MainViewModel _mainViewModel;
        private void OnMainWindowLoaded(Object sender, RoutedEventArgs e)
        {
            this._mainViewModel = new MainViewModel();

            this.DataContext = this._mainViewModel;
        }
    }
}
