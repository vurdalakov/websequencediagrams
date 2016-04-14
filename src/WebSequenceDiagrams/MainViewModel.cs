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

        public String LoadWsdScript { get; private set; }

        private void SetWsdScript(String wsdScript)
        {
            this.LoadWsdScript = wsdScript;
            this.OnPropertyChanged(() => this.LoadWsdScript);

            this.Refresh();
        }

        public WebSequenceDiagramsStyle Style
        {
            get
            {
                return (WebSequenceDiagramsStyle)this._settings.Get("WsdStyle", WebSequenceDiagramsStyle.Default);
            }
            set
            {
                if (value != this.Style)
                {
                    this._settings.Set("WsdStyle", value);
                    this.OnPropertyChanged(() => value);
                    this.Refresh();
                }
            }
        }

        private Int32 _currentLine = 1;
        public Int32 CurrentLine
        {
            get
            {
                return this._currentLine;
            }
            set
            {
                if (value != this._currentLine)
                {
                    this._currentLine = value;
                    this.OnPropertyChanged(() => this.CurrentLine);
                }
            }
        }

        private Int32 _currentColumn = 1;
        public Int32 CurrentColumn
        {
            get
            {
                return this._currentColumn;
            }
            set
            {
                if (value != this._currentColumn)
                {
                    this._currentColumn = value;
                    this.OnPropertyChanged(() => this.CurrentColumn);
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

        public Int32 MainWindowLeft
        {
            get
            {
                return this._settings.Get("MainWindowLeft", 64);
            }
            set
            {
                if (value != this.MainWindowLeft)
                {
                    this._settings.Set("MainWindowLeft", value);
                    this.OnPropertyChanged(() => this.MainWindowLeft);
                }
            }
        }

        public Int32 MainWindowTop
        {
            get
            {
                return this._settings.Get("MainWindowTop", 64);
            }
            set
            {
                if (value != this.MainWindowTop)
                {
                    this._settings.Set("MainWindowTop", value);
                    this.OnPropertyChanged(() => this.MainWindowWidth);
                }
            }
        }

        public Int32 MainWindowWidth
        {
            get
            {
                return this._settings.Get("MainWindowWidth", 1024);
            }
            set
            {
                if (value != this.MainWindowWidth)
                {
                    this._settings.Set("MainWindowWidth", value);
                    this.OnPropertyChanged(() => this.MainWindowWidth);
                }
            }
        }

        public Int32 MainWindowHeight
        {
            get
            {
                return this._settings.Get("MainWindowHeight", 768);
            }
            set
            {
                if (value != this.MainWindowHeight)
                {
                    this._settings.Set("MainWindowHeight", value);
                    this.OnPropertyChanged(() => this.MainWindowHeight);
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
                return String.Format("{0} - {1} {2}", this.ApplicationTitle, this.GetFileTitle(), this.DirtyFlag ? "*" : "");
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

        public BitmapImage WsdImage { get; private set; }

        private Int32 _actualWsdImageWidth;
        public Int32 ActualWsdImageWidth
        {
            get
            {
                return this._actualWsdImageWidth;
            }
            set
            {
                if (value != this._actualWsdImageWidth)
                {
                    this._actualWsdImageWidth = value;
                    this.OnPropertyChanged(() => this.ActualWsdImageWidth);

                    this.OnPropertyChanged(() => this.WsdImageWidth);
                }
            }
        }

        public Int32 WsdImageWidth
        {
            get
            {
                return Convert.ToInt32((float)this.ActualWsdImageWidth * this.Zoom / 100.0);
            }
        }

        private Int32 _zoom = 100;
        public Int32 Zoom
        {
            get
            {
                return this._zoom;
            }
            set
            {
                if (value != this._zoom)
                {
                    this._zoom = value;
                    this.OnPropertyChanged(() => this.Zoom);

                    this.OnPropertyChanged(() => this.WsdImageWidth);
                }
            }
        }

        public Boolean SyntaxHighlighting
        {
            get
            {
                return this._settings.Get("SyntaxHighlighting", true);
            }
            set
            {
                if (value != this.SyntaxHighlighting)
                {
                    this._settings.Set("SyntaxHighlighting", value);
                    this.OnPropertyChanged(() => this.SyntaxHighlighting);
                    this.OnPropertyChanged(() => this.SyntaxHighlightingFromResource);
                }
            }
        }

        public String ApiKey
        {
            get
            {
                return this._settings.Get("ApiKey", null);
            }
            set
            {
                if (value != this.ApiKey)
                {
                    this._settings.Set("ApiKey", value);
                }
            }
        }

        public Boolean OpenLastEditedFileOnStartup
        {
            get { return this._settings.Get("OpenLastEditedFileOnStartup", false); }
            set { this._settings.Set("OpenLastEditedFileOnStartup", value); }
        }

        public String NewFileContent
        {
            get { return this._settings.Get("NewFileContent", "title Simple Sequence Diagram\r\nClient->Server: send request\r\nServer->Client: return response").Replace("$$$$$", Environment.NewLine); }
            set { this._settings.Set("NewFileContent", value.Replace(Environment.NewLine, "$$$$$")); }
        }

        public String SyntaxHighlightingFromResource
        {
            get { return this.SyntaxHighlighting ? "Vurdalakov.WebSequenceDiagrams.SyntaxHighlighting.WebSequenceDiagrams.xshd" : null; }
        }

        public RecentFiles RecentFilesMenuItems { get; private set; }

        public ThreadSafeObservableCollection<MenuItemViewModel> PluginMenuItems { get; private set; }

        public ThreadSafeObservableCollection<ErrorViewModel> Errors { get; private set; }

        private PermanentSettings _settings;
        private Timer _timer;

        public MainViewModel(Window window) : base(window)
        {
            this._settings = new PermanentSettings();

            // File
            this.FileNewCommand = new CommandBase(this.OnFileNewCommand);
            this.FileOpenCommand = new CommandBase(this.OnFileOpenCommand);
            this.FileOpenRecentCommand = new CommandBase<String>(this.OnFileOpenRecentCommand);
            this.FileSaveCommand = new CommandBase(this.OnFileSaveCommand);
            this.FileSaveAsCommand = new CommandBase(this.OnFileSaveAsCommand);
            this.FileSaveImageAsCommand = new CommandBase(this.OnFileSaveImageAsCommand);
            this.ExitCommand = new CommandBase(this.OnExitCommand);
            // Edit
            // View
            this.ViewZoom100Command = new CommandBase(this.OnViewZoom100Command);
            this.ViewZoomInCommand = new CommandBase(this.OnViewZoomInCommand);
            this.ViewZoomOutCommand = new CommandBase(this.OnViewZoomOutCommand);
            this.RefreshCommand = new CommandBase(this.OnRefreshCommand);
            // Tools
            this.ToolsOptionsCommand = new CommandBase(this.OnToolsOptionsCommand);
            // Help
            this.AboutCommand = new CommandBase(this.OnAboutCommand);

            this.ErrorSelectedCommand = new CommandBase<Int32>(OnErrorSelectedCommand);

            this.Errors = new ThreadSafeObservableCollection<ErrorViewModel>();

            this._timer = new Timer(1000);
            this._timer.Elapsed += OnTimerElapsed;

            this.RecentFilesMenuItems = new RecentFiles(this._settings, this.FileOpenRecentCommand);

            this.PluginMenuItems = new ThreadSafeObservableCollection<MenuItemViewModel>();
            var plugins = PluginManager.FindPlugins();
            foreach (var plugin in plugins)
            {
                this.PluginMenuItems.Add(new MenuItemViewModel(plugin.GetMenuName(), new CommandBase(() => this.WsdScript = plugin.ModifyScript(this.WsdScript))));
            }
        }

        public override void OnMainWindowLoaded()
        {
            var lastEditedFile = this.RecentFilesMenuItems.GetMostRecentFile();
            if (!this.OpenLastEditedFileOnStartup || String.IsNullOrEmpty(lastEditedFile) || !OpenFile(lastEditedFile))
            {
                this.FileNewCommand.Execute(null);
            }
        }

        public override Boolean OnMainWindowClosing()
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

            SetWsdScript(this.NewFileContent);
        }

        public ICommand FileOpenCommand { get; private set; }
        public void OnFileOpenCommand()
        {
            if (!this.ConfirmSave())
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
                this.OpenFile(dlg.FileName);
            }
        }

        public ICommand FileOpenRecentCommand { get; private set; }
        public void OnFileOpenRecentCommand(String fileName)
        {
            if (!this.OpenFile(fileName))
            {
                this.RecentFilesMenuItems.RemoveFile(fileName);
            }
        }

        private Boolean OpenFile(String fileName)
        {
            try
            {
                SetWsdScript(File.ReadAllText(fileName));

                this.ScriptFilePath = fileName;

                this.RecentFilesMenuItems.AddFile(fileName);
                this._settings.Set("CurrentDirectory", Path.GetDirectoryName(fileName));

                return true;
            }
            catch (Exception ex)
            {
                MsgBox.Error(ex, "Cannot open file\n{0}", fileName);
                return false;
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

                try
                {
                    File.WriteAllText(this.ScriptFilePath, this.WsdScript);

                    this.DirtyFlag = false;

                    this.RecentFilesMenuItems.AddFile(dlg.FileName);
                    this._settings.Set("CurrentDirectory", Path.GetDirectoryName(dlg.FileName));

                    return true;
                }
                catch (Exception ex)
                {
                    MsgBox.Error(ex, "Cannot save file", dlg.FileName);
                    return false;
                }
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

        public ICommand ViewZoom100Command { get; private set; }
        public void OnViewZoom100Command()
        {
            this.Zoom = 100;
        }

        public ICommand ViewZoomInCommand { get; private set; }
        public void OnViewZoomInCommand()
        {
            if (this.Zoom < 800)
            {
                this.Zoom *= 2;
            }
        }

        public ICommand ViewZoomOutCommand { get; private set; }
        public void OnViewZoomOutCommand()
        {
            if (this.Zoom > 25)
            {
                this.Zoom /= 2;
            }
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

        public ICommand ToolsOptionsCommand { get; private set; }
        public void OnToolsOptionsCommand()
        {
            var optionsWindow = new OptionsWindow(Application.Current.MainWindow, this);
            optionsWindow.ShowDialog();
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

                this._webSequenceDiagramsResult = WebSequenceDiagrams.DownloadDiagram(this._wsdScript, this.Style.ToString().ToLower().Replace('_', '-'), "png", this.ApiKey);

                this.WsdImage = this._webSequenceDiagramsResult.GetBitmapImage();
                this.OnPropertyChanged(() => this.WsdImage);

                this.ActualWsdImageWidth = this._webSequenceDiagramsResult.ActualImageWidth;
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
            this.CurrentLine = lineNumber;
            this.CurrentColumn = 1;
            this.SetFocusOnScript = false;
            this.SetFocusOnScript = true;
        }
    }
}
