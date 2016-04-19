namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class RenameParticipantsPluginWindow : Window
    {
        public RenameParticipantsPluginWindow(Window owner, RenameParticipantsViewModel viewModel)
        {
            InitializeComponent();

            this.Loaded += (s, e) => { this.DataContext = viewModel; };

            this.Owner = owner;

            this.SetDialogStyle();
        }
    }
}
