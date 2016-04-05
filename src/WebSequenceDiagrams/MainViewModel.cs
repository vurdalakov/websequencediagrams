namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.IO;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using Microsoft.Win32;

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

        private void SetWsdScript(String wsdScript)
        {
            this._wsdScript = wsdScript;
            this.OnPropertyChanged(() => this.WsdScript);

            this.Refresh();
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

        private String _mainWindowTitle;
        public String MainWindowTitle
        {
            get
            {
                return String.Format("{0} - {1}", this.ApplicationTitleAndVersion, null == this.FileName ? "<New File>" : Path.GetFileName(this.FileName));
            }
        }

        private String _fileName;
        public String FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                if (value != this._fileName)
                {
                    this._fileName = value;
                    this.OnPropertyChanged(() => this.FileName);

                    this.OnPropertyChanged(() => this.MainWindowTitle);
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

            this.FileNewCommand = new CommandBase(this.OnFileNewCommand);
            this.FileOpenCommand = new CommandBase(this.OnFileOpenCommand);
            this.FileSaveCommand = new CommandBase(this.OnFileSaveCommand);
            this.FileSaveAsCommand = new CommandBase(this.OnFileSaveAsCommand);
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

            this.FileNewCommand.Execute(null);
        }

        public ICommand FileNewCommand { get; private set; }
        public void OnFileNewCommand()
        {
            this.FileName = null;
            this.OnPropertyChanged(() => this.MainWindowTitle);

            SetWsdScript("title Simple Sequence Diagram\r\nClient->Server: request image\r\nServer-> Client: return image");
        }

        public ICommand FileOpenCommand { get; private set; }
        public void OnFileOpenCommand()
        {
            var dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.DefaultExt = ".wsd";
            dlg.Filter = "WSD Files (*.wsd)|*.wsd|All Files (*.*)|*.*";
            dlg.InitialDirectory = this._settings.Get("CurrentDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            if ((Boolean)dlg.ShowDialog())
            {
                this.FileName = dlg.FileName;

                this._settings.Set("CurrentDirectory", Path.GetDirectoryName(this.FileName));

                SetWsdScript(File.ReadAllText(this.FileName));
            }
        }

        public ICommand FileSaveCommand { get; private set; }
        public void OnFileSaveCommand()
        {
            File.WriteAllText(_fileName, this.WsdScript);
        }

        public ICommand FileSaveAsCommand { get; private set; }
        public void OnFileSaveAsCommand()
        {
            var dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            dlg.DefaultExt = ".wsd";
            dlg.Filter = "WSD Files (*.wsd)|*.wsd|All Files (*.*)|*.*";
            dlg.InitialDirectory = this._settings.Get("CurrentDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            dlg.OverwritePrompt = true;

            if ((Boolean)dlg.ShowDialog())
            {
                this.FileName = dlg.FileName;

                this._settings.Set("CurrentDirectory", Path.GetDirectoryName(this.FileName));

                File.WriteAllText(this.FileName, this.WsdScript);
            }
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
