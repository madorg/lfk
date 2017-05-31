using LfkClient.UserMessages;
using LfkGUI.Services;
using LfkSharedResources.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryHistoryCommandViewModel : BasicViewModel
    {
        private WindowsService windowsService;
        private DialogService dialogService;
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();

        public RepositoryHistoryCommandViewModel(WindowsService windowsService)
        {
            dialogService = new DialogService();
            this.windowsService = windowsService;
            Commits = new ObservableCollection<Commit>();
            foreach (var item in Repository.History())
            {
                Commits.Add(item);
            }
        }

        public ObservableCollection<Commit> Commits { get; set; }

        private RelayCommand switchCommand;
        public RelayCommand SwitchCommand
        {
            get
            {
                return switchCommand ?? (switchCommand = new RelayCommand(Switch, obj =>
                {
                    return obj != null;
                }
                ));
            }
        }

        private  async void Switch(object obj)
        {
            Commit commit = obj as Commit;

            InvalidCommitSwitchingReasons reason =  Repository.CanSwitch(commit);
            switch (reason)
            {
                case InvalidCommitSwitchingReasons.None:
                    SwitchToCommit(commit);
                    break;
                case InvalidCommitSwitchingReasons.NotCommittedChanges:
                    if(await dialogService.ShowMessageDialog(windowsService.CurrentOpenedWindow,"Вы уверены что хотите переключиться на коммит без сохранения изменений?"))
                    {
                        SwitchToCommit(commit);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SwitchToCommit(Commit commit)
        {
            dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "Успешное переключение на коммит : \n" + commit.Id.ToString() +
                "\n" + "Сообщение : " + commit.Comment);
        }
    }
}
