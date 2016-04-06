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
