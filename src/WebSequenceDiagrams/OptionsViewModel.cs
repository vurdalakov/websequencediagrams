namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows.Input;

    public class OptionsViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel;

        public Boolean OpenLastEditedFileOnStartup { get; set; }
        public String NewFileContent { get; set; }

        public Boolean SyntaxHighlighting { get; set; }

        public OptionsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            this.OkCommand = new CommandBase(this.OnOkCommand);

            this.OpenLastEditedFileOnStartup = this.mainViewModel.OpenLastEditedFileOnStartup;
            this.NewFileContent = this.mainViewModel.NewFileContent;
            this.SyntaxHighlighting = this.mainViewModel.SyntaxHighlighting;
        }

        public ICommand OkCommand { get; private set; }
        public void OnOkCommand()
        {
            this.mainViewModel.OpenLastEditedFileOnStartup = this.OpenLastEditedFileOnStartup;
            this.mainViewModel.NewFileContent = this.NewFileContent;
            this.mainViewModel.SyntaxHighlighting = this.SyntaxHighlighting;
        }
    }
}
