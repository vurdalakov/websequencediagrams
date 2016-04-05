namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
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

                    this._settings.Set("WsdStyle", this._style);
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

        private Int32 _lineNumber = 1;
        public Int32 LineNumber
        {
            get
            {
                return this._lineNumber;
            }
            set
            {
                if (value != this._lineNumber)
                {
                    this._lineNumber = value;
                    this.OnPropertyChanged(() => this.LineNumber);
                }
            }
        }

        private Int32 _columnNumber = 1;
        public Int32 ColumnNumber
        {
            get
            {
                return this._columnNumber;
            }
            set
            {
                if (value != this._columnNumber)
                {
                    this._columnNumber = value;
                    this.OnPropertyChanged(() => this.ColumnNumber);
                }
            }
        }

        private Boolean _setFocusOnScript = false;
        public Boolean SetFocusOnScript
        {
            get
            {
                return this._setFocusOnScript;
            }
            set
            {
                if (value != this._setFocusOnScript)
                {
                    this._setFocusOnScript = value;
                    this.OnPropertyChanged(() => this.SetFocusOnScript);
                }
            }
        }

        private Int32 _mainWindowLeft = 64;
        public Int32 MainWindowLeft
        {
            get
            {
                return this._mainWindowLeft;
            }
            set
            {
                if (value != this._mainWindowLeft)
                {
                    this._mainWindowLeft = value;
                    this.OnPropertyChanged(() => this.MainWindowLeft);

                    this._settings.Set("MainWindowLeft", this._mainWindowLeft);
                }
            }
        }

        private Int32 _mainWindowTop = 64;
        public Int32 MainWindowTop
        {
            get
            {
                return this._mainWindowTop;
            }
            set
            {
                if (value != this._mainWindowTop)
                {
                    this._mainWindowTop = value;
                    this.OnPropertyChanged(() => this.MainWindowWidth);

                    this._settings.Set("MainWindowTop", this._mainWindowTop);
                }
            }
        }

        private Int32 _mainWindowWidth = 1024;
        public Int32 MainWindowWidth
        {
            get
            {
                return this._mainWindowWidth;
            }
            set
            {
                if (value != this._mainWindowWidth)
                {
                    this._mainWindowWidth = value;
                    this.OnPropertyChanged(() => this.MainWindowWidth);

                    this._settings.Set("MainWindowWidth", this._mainWindowWidth);
                }
            }
        }

        private Int32 _mainWindowHeight = 768;
        public Int32 MainWindowHeight
        {
            get
            {
                return this._mainWindowHeight;
            }
            set
            {
                if (value != this._mainWindowHeight)
                {
                    this._mainWindowHeight = value;
                    this.OnPropertyChanged(() => this.MainWindowHeight);

                    this._settings.Set("MainWindowHeight", this._mainWindowHeight);
                }
            }
        }

        public ThreadSafeObservableCollection<ErrorViewModel> Errors { get; private set; }

        private PermanentSettings _settings;
        private Timer _timer;

        public MainViewModel()
        {
            this._settings = new PermanentSettings();
            this._style = (WebSequenceDiagramsStyle)this._settings.Get("WsdStyle", WebSequenceDiagramsStyle.Default);

            this.ExitCommand = new CommandBase(this.OnExitCommand);
            this.RefreshCommand = new CommandBase(this.OnRefreshCommand);
            this.AboutCommand = new CommandBase(this.OnAboutCommand);

            this.ErrorSelectedCommand = new CommandBase<Int32>(OnErrorSelectedCommand);

            this.Errors = new ThreadSafeObservableCollection<ErrorViewModel>();

            this._timer = new Timer(1000);
            this._timer.Elapsed += OnTimerElapsed;

            this.MainWindowLeft = this._settings.Get("MainWindowLeft", 64);
            this.MainWindowTop = this._settings.Get("MainWindowTop", 64);
            this.MainWindowWidth = this._settings.Get("MainWindowWidth", 1024);
            this.MainWindowHeight = this._settings.Get("MainWindowHeight", 768);

            this._wsdScript = "title Simple Sequence Diagram\r\nClient->Server: request image\r\nServer-> Client: return image";
            this.Refresh();
        }

        public ICommand ExitCommand { get; private set; }
        public void OnExitCommand()
        {
            Application.Current.MainWindow.Close();
        }

        public ICommand RefreshCommand { get; private set; }
        public void OnRefreshCommand()
        {
            this.Refresh();
        }

        public ICommand AboutCommand { get; private set; }
        public void OnAboutCommand()
        {
            var aboutWindow = new AboutWindow(Application.Current.MainWindow);
            aboutWindow.ShowDialog();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this._timer.Stop();
            this.Refresh();
        }

        private void Refresh()
        {
            try
            {
                this.Errors.Clear();
                this.WsdImage = WebSequenceDiagrams.GetDiagram(this._wsdScript, this._style.ToString().ToLower().Replace('_', '-'), "png");
            }
            catch (WebSequenceDiagramsException ex)
            {
                foreach (var error in ex.Errors)
                {
                    this.Errors.Add(new ErrorViewModel(error.Line, error.Message));
                }
            }
            catch (Exception ex)
            {
                this.Errors.Add(new ErrorViewModel(0, ex.Message));
            }
        }

        public ICommand ErrorSelectedCommand { get; private set; }
        private void OnErrorSelectedCommand(Int32 lineNumber)
        {
            this.LineNumber = lineNumber;
            this.SetFocusOnScript = false;
            this.SetFocusOnScript = true;
        }
    }
}
