namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class OptionsWindow : Window
    {
        public OptionsWindow(Window owner, MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.Loaded += (s, e) => { this.DataContext = new OptionsViewModel(mainViewModel); };

            this.Owner = owner;

            this.SetDialogStyle();
        }
    }
}
