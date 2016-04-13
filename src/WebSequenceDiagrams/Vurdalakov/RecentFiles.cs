namespace Vurdalakov
{
    using System;
    using System.Windows.Input;

    public class RecentFile : MenuItemViewModel
    {
        public RecentFile(String fileName, ICommand command) : base(fileName, command, fileName)
        {
        }

        public String FileName { get { return this.Header; } }

        public Boolean Equals(String fileName)
        {
            return this.FileName.Equals(fileName, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public class RecentFiles : ThreadSafeObservableCollection<RecentFile>
    {
        private readonly PermanentSettings permanentSettings;
        private readonly ICommand command;

        public Int32 MaxFiles { get; private set; }

        public RecentFiles(PermanentSettings permanentSettings, ICommand command)
        {
            this.permanentSettings = permanentSettings;
            this.command = command;

            this.MaxFiles = 5;

            this.Read();
        }

        public void AddFile(String fileName)
        {
            this.RemoveFile(fileName);

            this.Insert(0, new RecentFile(fileName, this.command));

            while (this.Count > this.MaxFiles)
            {
                this.RemoveAt(this.Count - 1);
            }

            this.Save();
        }

        public void RemoveFile(String fileName)
        {
            foreach (var recentFile in this)
            {
                if (recentFile.Equals(fileName))
                {
                    this.Remove(recentFile);
                    break;
                }
            }

            this.Save();
        }

        public String GetMostRecentFile() { return this.Count > 0 ? this[0].FileName : null; }

        private void Read()
        {
            this.Clear();

            for (var i = 0; i < this.MaxFiles; i++)
            {
                var fileName = this.permanentSettings.Get(GetSettingName(i), null);
                if (!String.IsNullOrEmpty(fileName))
                {
                    this.Add(new RecentFile(fileName, this.command));
                }
            }
        }

        private void Save()
        {
            for (var i = 0; i < this.MaxFiles; i++)
            {
                this.permanentSettings.Set(GetSettingName(i), this.Count > i ? this[i].FileName : "");
            }
        }

        private String GetSettingName(Int32 i)
        {
            return String.Format("RecentFile{0}", i);
        }
    }
}
