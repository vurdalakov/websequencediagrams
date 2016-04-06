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

                    this.DirtyFlag = true;
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

        public BitmapImage WsdImage { get; private set; }
        public Int32 WsdImageWidth { get; private set; }

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

        private String GetFileTitle()
        {
            return null == this.ScriptFilePath ? "<New File>" : Path.GetFileName(this.ScriptFilePath);
        }

        public String MainWindowTitle
        {
            get
            {
                return String.Format("{0} - {1} {2}", this.ApplicationTitleAndVersion, this.GetFileTitle(), this.DirtyFlag ? "*" : "");
            }
        }

        public String ScriptFileName { get; private set; }

        private String _scriptFilePath;
        public String ScriptFilePath
        {
            get
            {
                return this._scriptFilePath;
            }
            set
            {
                if (value != this._scriptFilePath)
                {
                    this._scriptFilePath = value;
                    this.OnPropertyChanged(() => this.ScriptFilePath);

                    this.ScriptFileName = Path.GetFileName(this._scriptFilePath);
                    this.OnPropertyChanged(() => this.ScriptFileName);

                    this.OnPropertyChanged(() => this.MainWindowTitle);
                }
            }
        }

        private Boolean _dirtyFlag = false;
        public Boolean DirtyFlag
        {
            get
            {
                return this._dirtyFlag;
            }
            set
            {
                if (value != this._dirtyFlag)
                {
                    this._dirtyFlag = value;
                    this.OnPropertyChanged(() => this.DirtyFlag);

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
            this.FileSaveImageAsCommand = new CommandBase(this.OnFileSaveImageAsCommand);
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

        public Boolean OnMainWindowClosing()
        {
            return !ConfirmSave();
        }

        public ICommand FileNewCommand { get; private set; }
        public void OnFileNewCommand()
        {
            if (!ConfirmSave())
            {
                return;
            }

            this.ScriptFilePath = null;
            this.OnPropertyChanged(() => this.MainWindowTitle);

            SetWsdScript("title Simple Sequence Diagram\r\nClient->Server: send request\r\nServer-> Client: return response");
        }

        public ICommand FileOpenCommand { get; private set; }
        public void OnFileOpenCommand()
        {
            if (!ConfirmSave())
            {
                return;
            }

            var dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            dlg.DefaultExt = ".wsd";
            dlg.Filter = "WSD Files (*.wsd)|*.wsd|All Files (*.*)|*.*";
            dlg.InitialDirectory = this._settings.Get("CurrentDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            if ((Boolean)dlg.ShowDialog())
            {
                this.ScriptFilePath = dlg.FileName;

                this._settings.Set("CurrentDirectory", Path.GetDirectoryName(this.ScriptFilePath));

                SetWsdScript(File.ReadAllText(this.ScriptFilePath));
                this.DirtyFlag = false;
            }
        }

        public ICommand FileSaveCommand { get; private set; }
        public void OnFileSaveCommand()
        {
            if (this._scriptFilePath != null)
            {
                File.WriteAllText(_scriptFilePath, this.WsdScript);
                this.DirtyFlag = false;
            }
        }

        public ICommand FileSaveAsCommand { get; private set; }
        public void OnFileSaveAsCommand()
        {
            SaveFileAs();
        }

        public Boolean SaveFileAs()
        {
            var dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            dlg.DefaultExt = ".wsd";
            dlg.Filter = "WSD Files (*.wsd)|*.wsd";
            dlg.InitialDirectory = this._settings.Get("CurrentDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            dlg.OverwritePrompt = true;

            if ((Boolean)dlg.ShowDialog())
            {
                this.ScriptFilePath = dlg.FileName;

                this._settings.Set("CurrentDirectory", Path.GetDirectoryName(this.ScriptFilePath));

                File.WriteAllText(this.ScriptFilePath, this.WsdScript);
                this.DirtyFlag = false;

                return true;
            }

            return false;
        }

        private Boolean ConfirmSave()
        {
            if (!this.DirtyFlag)
            {
                return true;
            }

            switch (MessageBox.Show(Application.Current.MainWindow, String.Format("Save file {0}?", this.GetFileTitle()), "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }

            if (null == this.ScriptFilePath)
            {
                return SaveFileAs();
            }
            else
            {
                this.FileSaveCommand.Execute(null);
                return true;
            }
        }

        
        public ICommand FileSaveImageAsCommand { get; private set; }
        public void OnFileSaveImageAsCommand()
        {
            var dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp|Graphics Interchange Format (*.gif)|*.gif|JPEG / JFIF (*.jpg)|*.jpg|Portable Network Graphics (*.png)|*.png|Tagged Image File Format (*.tiff)|*.tiff";
            dlg.FilterIndex = this._settings.Get("FileSaveImageAsFilterIndex", 3);
            dlg.InitialDirectory = this._settings.Get("CurrentDirectory", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            dlg.OverwritePrompt = true;

            if ((Boolean)dlg.ShowDialog())
            {
                this._settings.Set("CurrentDirectory", Path.GetDirectoryName(dlg.FileName));
                this._settings.Set("FileSaveImageAsFilterIndex", dlg.FilterIndex);

                this._webSequenceDiagramsResult.SaveImage(dlg.FileName, dlg.FilterIndex);
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

        private WebSequenceDiagramsResult _webSequenceDiagramsResult;

        private void Refresh()
        {
            try
            {
                this.Errors.Clear();

                this._webSequenceDiagramsResult = WebSequenceDiagrams.DownloadDiagram(this._wsdScript, this._style.ToString().ToLower().Replace('_', '-'), "png");

                this.WsdImage = this._webSequenceDiagramsResult.GetBitmapImage();
                this.OnPropertyChanged(() => this.WsdImage);

                this.WsdImageWidth = this._webSequenceDiagramsResult.ActualImageWidth;
                this.OnPropertyChanged(() => this.WsdImageWidth);
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
