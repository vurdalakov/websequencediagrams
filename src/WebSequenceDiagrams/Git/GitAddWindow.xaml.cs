namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class GitAddWindow : Window
    {
        public GitAddWindow(Window owner, ViewModelBase viewModelBase)
        {
            InitializeComponent();

            this.Loaded += (s, e) => { this.DataContext = viewModelBase; };

            this.Owner = owner;

            this.SetDialogStyle();
        }
    }
}
