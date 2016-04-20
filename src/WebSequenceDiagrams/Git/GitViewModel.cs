namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Windows;
    using System.IO;
    using LibGit2Sharp;

    public class GitViewModel : ViewModelBase
    {
        private Repository repository;

        public GitViewModel(String fileOrDirectoryName)
        {
            this.repository = new Repository(Path.GetDirectoryName(fileOrDirectoryName));
        }

        public String GetUserName()
        {
            return this.repository.Config.Get<String>("user.name").Value;
        }

        public String GetUserEmail()
        {
            return this.repository.Config.Get<String>("user.email").Value;
        }

        public FileStatus GetFileStatus(String fileName)
        {
            return this.repository.RetrieveStatus(fileName);
        }

        public void AddFile(String fileName, String commitMessage)
        {
            // add
            this.repository.Index.Add(fileName);

            // stage and commit
            this.CommitFile(fileName, commitMessage);
        }

        public void CommitFile(String fileName, String commitMessage)
        {
            // committer
            var committer = new Signature(this.GetUserName(), this.GetUserEmail(), DateTime.Now);

            // stage
            this.repository.Stage(fileName);

            // commit
            this.repository.Commit(commitMessage, committer, committer);
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
