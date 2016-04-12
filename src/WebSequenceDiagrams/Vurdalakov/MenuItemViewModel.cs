namespace Vurdalakov
{
    using System;
    using System.Windows.Input;

    public class MenuItemViewModel : ViewModelBase
    {
        public MenuItemViewModel(String header, ICommand command)
        {
            this.Header = header;
            this.Command = command;
        }

        public String Header { get; set; }

        public ICommand Command { get; set; }
    }
}
