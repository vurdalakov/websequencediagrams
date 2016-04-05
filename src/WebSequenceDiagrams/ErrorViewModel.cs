namespace Vurdalakov.WebSequenceDiagrams
{
    using System;

    public class ErrorViewModel : ViewModelBase
    {
        public Int32 LineNumber { get; private set; }
        public String Message { get; private set; }

        public ErrorViewModel(Int32 lineNumber, String message)
        {
            this.LineNumber = lineNumber;
            this.Message = message;
        }
    }
}
