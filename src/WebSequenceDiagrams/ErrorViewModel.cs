namespace Vurdalakov.WebSequenceDiagrams
{
    using System;

    public class ErrorViewModel : ViewModelBase
    {
        public Int32 Line { get; private set; }
        public String Message { get; private set; }

        public ErrorViewModel(Int32 line, String message)
        {
            this.Line = line;
            this.Message = message;
        }
    }
}
