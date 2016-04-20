namespace Vurdalakov.WebSequenceDiagrams
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    public class ParticipantViewModel : ViewModelBase
    {
        public String Name { get; private set; }

        public ParticipantViewModel(String name)
        {
            this.Name = name;
        }
    }

    public class RenameParticipantsViewModel : ViewModelBase
    {
        private Participants participants;

        private Boolean isUser = true;
        public Boolean IsUser
        {
            get
            {
                return this.isUser;
            }
            set
            {
                if (value != this.isUser)
                {
                    this.isUser = value;
                    this.OnPropertyChanged(() => this.IsUser);

                    this.PopulateOldUsers(this.isUser);
                }
            }
        }

        public Boolean AliasesAvailable { get; private set; }
        public ThreadSafeObservableCollection<ParticipantViewModel> OldUsers { get; private set; }
        public ParticipantViewModel OldUser { get; set; }

        public String OldName { get { return this.OldUser.Name; } }

        private String newName = "";
        public String NewName
        {
            get
            {
                return this.newName;
            }
            set
            {
                if (value != this.newName)
                {
                    this.newName = value;
                    this.OnPropertyChanged(() => this.NewName);
                }
            }
        }

        public RenameParticipantsViewModel(Participants participants)
        {
            this.participants = participants;

            this.AliasesAvailable = participants.AliasesCount > 0;
            this.OldUsers = new ThreadSafeObservableCollection<ParticipantViewModel>();
            this.PopulateOldUsers(true);
        }

        private void PopulateOldUsers(Boolean user)
        {
            this.OldUsers.Clear();

            foreach (var participant in this.participants)
            {
                if (user)
                {
                    this.OldUsers.Add(new ParticipantViewModel(participant.Key));
                }
                else if (!String.IsNullOrEmpty(participant.Value))
                {
                    this.OldUsers.Add(new ParticipantViewModel(participant.Value));
                }
            }

            this.OldUser = this.OldUsers.First();
            this.OnPropertyChanged(() => this.OldUser);
        }
    }
}
