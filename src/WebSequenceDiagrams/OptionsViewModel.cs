namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows.Input;

    public class OptionsViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel;

        public Boolean SyntaxHighlighting { get; set; }

        public OptionsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            this.OkCommand = new CommandBase(this.OnOkCommand);

            this.SyntaxHighlighting = this.mainViewModel.SyntaxHighlightingFromResource != null;
        }

        public ICommand OkCommand { get; private set; }
        public void OnOkCommand()
        {
            this.mainViewModel.SyntaxHighlightingFromResource = this.SyntaxHighlighting ? "Vurdalakov.WebSequenceDiagrams.SyntaxHighlighting.WebSequenceDiagrams.xshd" : null;


        }
    }
}
