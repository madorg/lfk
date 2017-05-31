using LfkGUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryCommitCommandViewModel : BasicViewModel
    {
        private DialogService dialogService;
        private WindowsService windowService;
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();

        public RepositoryCommitCommandViewModel(WindowsService windowService)
        {
            dialogService = new DialogService();
            FilesToBeCommitted = new ObservableCollection<string>();
            this.windowService = windowService;
            UpdateFilesToBeCommitted();
        }

        public async void UpdateFilesToBeCommitted()
        {
            foreach (var item in await Repository.GetChangedFilesAfterParentCommit())
            {
                FilesToBeCommitted.Add(item);
            }
        }

        public ObservableCollection<string> FilesToBeCommitted { get; set; }

        private string commitMessage;
        public string CommitMessage
        {
            get
            {
                return commitMessage;
            }
            set
            {
                commitMessage = value;
                OnPropertyChanged("CommitMessage");
            }
        }

        private RelayCommand commitCommand;
        public RelayCommand CommitCommand
        {
            get
            {
                return commitCommand ?? (commitCommand = new RelayCommand(Commit, obj =>
                {
                    return FilesToBeCommitted.Count != 0 && !string.IsNullOrWhiteSpace(obj.ToString());
                }
                ));
            }
        }

        private void Commit(object obj)
        {
            if (string.IsNullOrWhiteSpace(CommitMessage))
            {
                dialogService.ShowMessage(windowService.CurrentOpenedWindow, "Введите сообщение для коммита");
                return;
            }
            FilesToBeCommitted.Clear();
            Repository.Commit(CommitMessage);
            CommitMessage = string.Empty;

        }
    }
}
