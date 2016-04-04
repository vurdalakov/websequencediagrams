namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Timers;
    using System.Windows.Media.Imaging;

    public class MainViewModel : ViewModelBase
    {
        private String _wsdScript;
        public String WsdScript
        {
            get
            {
                return this._wsdScript;
            }
            set
            {
                if (value != this._wsdScript)
                {
                    this._wsdScript = value;
                    this.OnPropertyChanged(() => this.WsdScript);

                    this._timer.Stop();
                    this._timer.Start();
                }
            }
        }

        private WebSequenceDiagramsStyle _style;
        public WebSequenceDiagramsStyle Style
        {
            get
            {
                return this._style;
            }
            set
            {
                if (value != this._style)
                {
                    this._style = value;
                    this.OnPropertyChanged(() => this.Style);

                    this.Refresh();
                }
            }
        }

        private BitmapImage _wsdImage;
        public BitmapImage WsdImage
        {
            get
            {
                return this._wsdImage;
            }
            set
            {
                if (value != this._wsdImage)
                {
                    this._wsdImage = value;
                    this.OnPropertyChanged(() => this.WsdImage);
                }
            }
        }

        private Timer _timer;

        public MainViewModel()
        {
            this._style = WebSequenceDiagramsStyle.Default;

            this._wsdScript = "title Simple Sequence Diagram\r\nClient->Server: request image\r\nServer-- > Client: return image";
            this.Refresh();

            this._timer = new Timer(1000);
            this._timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._timer.Stop();
            this.Refresh();
        }

        private void Refresh()
        {
            this.WsdImage = WebSequenceDiagrams.GetDiagram(this._wsdScript, this._style.ToString().ToLower().Replace('_', '-'), "png");
        }
    }
}
