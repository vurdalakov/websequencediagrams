namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if ((Environment.CommandLine.IndexOf("-reset") >= 0) || (Environment.CommandLine.IndexOf("/reset") >= 0))
            {
                PermanentSettings.Reset();
            }

            base.OnStartup(e);
        }
    }
}
