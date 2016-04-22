namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
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

        public Boolean AreRemotesAvailable()
        {
            foreach (var remote in this.Repository.Network.Remotes)
            {
                return true;
            }

            return false;
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

        public void Push(String remoteName, String branchName, String userName, String password)
        {
            var remote = this.Repository.Network.Remotes[remoteName];
            var branch = this.Repository.Branches[branchName].CanonicalName; // "refs/heads/master"

            var pushOptions = new PushOptions();
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                pushOptions.CredentialsProvider = (url, username, types) => new UsernamePasswordCredentials { Username = userName, Password = password };
            }

            this.Repository.Network.Push(remote, branch, pushOptions);
        }

        public void Pull(String userName, String password)
        {
            var pullOptions = new PullOptions();
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                pullOptions.FetchOptions.CredentialsProvider = (url, username, types) => new UsernamePasswordCredentials { Username = userName, Password = password };
            }

            var committer = new Signature(this.GetUserName(), this.GetUserEmail(), DateTime.Now);

            var mergeResult = this.Repository.Network.Pull(committer, pullOptions);

            if (MergeStatus.Conflicts == mergeResult.Status)
            {
                throw new Exception("Merge resulted in conflicts.");
            }
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

        #region ShowRemoteDialog

        public String RemoteWindowTitle { get; private set; }
        public List<String> RemoteNames { get; private set; }
        public String RemoteName { get; set; }
        public String BranchName { get; private set; }
        public String UserName { get; set; }
        public String Password { get; set; }

        public Boolean ShowRemoteDialog(String dialogTitle)
        {
            this.RemoteWindowTitle = dialogTitle;
            this.BranchName = this.Repository.Head.FriendlyName;
            this.UserName = this.GetUserName();

            this.RemoteNames = new List<String>();
            foreach (var remote in this.Repository.Network.Remotes)
            {
                this.RemoteNames.Add(remote.Name);
            }
            this.RemoteName = this.RemoteNames[0];

            var dlg = new GitRemoteWindow(Application.Current.MainWindow, this);
            dlg.ShowDialog();

            return dlg.DialogResult.HasValue && dlg.DialogResult.Value;
        }

        #endregion
    }
}
