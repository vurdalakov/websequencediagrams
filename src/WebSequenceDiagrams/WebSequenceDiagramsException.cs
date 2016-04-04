namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class WebSequenceDiagramsError
    {
        public Int32 Line { get; private set; }
        public String Message { get; private set; }

        public WebSequenceDiagramsError(String error)
        {
            try
            {
                this._error = error;

                var parts1 = error.Split(':');
                var parts2 = parts1[0].Split(' ');

                this.Line = Int32.Parse(parts2[1]);
                this.Message = parts1[1].Trim();
            }
            catch
            {
                this.Line = 0;
                this.Message = error;
            }
        }

        private String _error;

        public override String ToString()
        {
            return this._error;
        }
    }

    public class WebSequenceDiagramsException : Exception
    {
        public WebSequenceDiagramsError[] Errors { get; private set; }

        private WebSequenceDiagramsException(JArray errors) : base(errors.ToString().Replace('[', ' ').Replace(']', ' ').Replace('"', ' ').Trim())
        {
            var errorList = new List<WebSequenceDiagramsError>();
            foreach (var error in errors)
            {
                errorList.Add(new WebSequenceDiagramsError(error.ToObject<String>()));
            }

            this.Errors = errorList.ToArray();
        }

        public static void CheckResult(JObject root)
        {
            if (root == null)
            {
                throw new ArgumentNullException();
            }

            var errors = root.GetValue("errors") as JArray;
            if ((errors != null) && (errors.Count > 0))
            {
                throw new WebSequenceDiagramsException(errors);
            }
        }
    }
}
