namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;
    using System.IO;
    using LibGit2Sharp;

    public class GitViewModel : ViewModelBase
    {
        public Repository Repository { get; private set; }

        public GitViewModel(String fileOrDirectoryName)
        {
            var workingDir = new DirectoryInfo(
                File.Exists(fileOrDirectoryName) ? Path.GetDirectoryName(fileOrDirectoryName) : fileOrDirectoryName);

            while ((workingDir != null) && (workingDir.GetDirectories(".git").Length != 1))
            {
                workingDir = workingDir.Parent;
            }

            if (null == workingDir)
            {
                throw new Exception("Git Repository not found");
            }

            this.Repository = new Repository(workingDir.FullName);
        }

        public String GetUserName()
        {
            return this.Repository.Config.Get<String>("user.name").Value;
        }

        public String GetUserEmail()
        {
            return this.Repository.Config.Get<String>("user.email").Value;
        }

        public FileStatus GetFileStatus(String fileName)
        {
            return this.Repository.RetrieveStatus(fileName);
        }

        public void AddFile(String fileName, String commitMessage)
        {
            // add
            this.Repository.Index.Add(fileName);

            // stage and commit
            this.CommitFile(fileName, commitMessage);
        }

        public void CommitFile(String fileName, String commitMessage)
        {
            // committer
            var committer = new Signature(this.GetUserName(), this.GetUserEmail(), DateTime.Now);

            // stage
            this.Repository.Stage(fileName);

            // commit
            this.Repository.Commit(commitMessage, committer, committer);
        }

        #region ShowCommitMessageDialog

        public String CommitMessageWindowTitle { get; private set; }
        public String CommitterInfo { get; private set; }

        private String commitMessage = "";
        public String CommitMessage
        {
            get
            {
                return this.commitMessage;
            }
            set
            {
                if (value != this.commitMessage)
                {
                    this.commitMessage = value;
                    this.OnPropertyChanged(() => this.CommitMessage);
                }
            }
        }

        public String ShowCommitMessageDialog(String dialogTitle)
        {
            this.CommitMessageWindowTitle = dialogTitle;
            this.CommitterInfo = String.Format("Committer {0} {1}", this.GetUserName(), this.GetUserEmail());
            this.CommitMessage = "";

            var dlg = new GitCommitMessageWindow(Application.Current.MainWindow, this);
            dlg.ShowDialog();

            if (!dlg.DialogResult.HasValue || !dlg.DialogResult.Value)
            {
                return null;
            }

            return this.CommitMessage;
        }

        #endregion
    }
}
