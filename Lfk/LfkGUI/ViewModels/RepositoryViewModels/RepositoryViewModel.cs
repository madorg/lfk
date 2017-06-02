using LfkExceptions;
using LfkGUI.Services;
using LfkGUI.Views.RepositoryManagementViews;
using LfkSharedResources.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryViewModel : BasicViewModel
    {
        private WindowsService windowsService;
        private DialogService dialogService;
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();

        public RepositoryViewModel(WindowsService windowsService)
        {
            dialogService = new DialogService();
            this.windowsService = windowsService;
        }


        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ?? (updateCommand = new RelayCommand((obj)=>
                {
                    try
                    {
                        Repository.Update();
                        dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "Репозиторий успешно загружен на сервер");
                    }
                    catch (RepositoryUpdateWithoutCommitsException ruwce)
                    {
                        dialogService.ShowMessage(windowsService.CurrentOpenedWindow, ruwce.Message);
                    }
                    
                }));
            }
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return backCommand ?? (backCommand = new RelayCommand((obj) =>
                {
                    windowsService.NavigateToWindow(new RepositoryManagementWindow());
                }));
            }
        }

    }
}