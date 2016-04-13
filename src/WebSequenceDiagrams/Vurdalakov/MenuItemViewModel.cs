namespace Vurdalakov
{
    using System;
    using System.Windows.Input;

    public class MenuItemViewModel : ViewModelBase
    {
        public MenuItemViewModel(String header, ICommand command, Object commandParameter = null)
        {
            this.Header = header;
            this.Command = command;
            this.CommandParameter = commandParameter;
        }

        public String Header { get; set; }

        public ICommand Command { get; set; }

        public Object CommandParameter { get; set; }
    }
}
