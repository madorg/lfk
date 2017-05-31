using LfkClient.UserMessages;
using LfkGUI.Services;
using LfkSharedResources.Models.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkGUI.Views.RepositoryViews;
using MahApps.Metro.Controls;

namespace LfkGUI.ViewModels.RepositoryManagementViewModels
{
    public class RepositoryManagementViewModel : BasicViewModel
    {
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();
        private FolderOpenDialogService openFolderService;
        private DialogService dialogService;
        private WindowsService windowsService;

        public RepositoryManagementViewModel(WindowsService windowService)
        {
            ManagedRepositories = new ObservableCollection<LocalRepository>();
            openFolderService = new FolderOpenDialogService();
            dialogService = new DialogService();
            windowsService = windowService;
            LocalRepository = new LocalRepository();
            SelectedRepository = new LocalRepository();
        }

        #region Свойства
        public ObservableCollection<LocalRepository> ManagedRepositories { get; set; }

        public LocalRepository LocalRepository { get; set; }

        public LocalRepository SelectedRepository { get; set; }

        public string Title
        {
            get
            {
                return LocalRepository.Title;
            }
            set
            {
                LocalRepository.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Path
        {
            get
            {
                return LocalRepository.Path;
            }
            set
            {
                LocalRepository.Path = value;
                OnPropertyChanged("Path");
            }
        }

        #endregion

        #region Команды

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ?? (createCommand = new RelayCommand(Create,obj=> {
                    return !string.IsNullOrWhiteSpace(LocalRepository.Title) && System.IO.Directory.Exists(Path);
                    }));
            }
        }


        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand(Delete, obj => SelectedRepository.Id != Guid.Empty));
            }
        }

        private RelayCommand downloadCommand;
        public RelayCommand DownloadCommand
        {
            get
            {
                return downloadCommand ?? (downloadCommand = new RelayCommand(Download, obj => SelectedRepository.Id != Guid.Empty));
            }
        }

        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ?? (openCommand = new RelayCommand(Open));
            }
        }

        private RelayCommand showCommand;
        public RelayCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new RelayCommand(Show));
            }
        }

        private RelayCommand chooseFolderCommand;
        public RelayCommand ChooseFolderCommand
        {
            get
            {
                return chooseFolderCommand ?? (chooseFolderCommand = new RelayCommand(obj=> {
                    if (openFolderService.OpenFolderDialog())
                    {
                        Path = openFolderService.FilePath;
                    }
                }));
            }
        }
        #endregion

        #region Методы обработки комманд

        private void Show(object obj)
        {
            foreach (var item in Repository.GetManagedRepositories(App.User.Id.ToString()))
            {
                if (!ManagedRepositories.Any(lr=>lr.Id == item.Id))
                    ManagedRepositories.Add(item);
            }
        }

        private void Open(object obj)
        {
            if (openFolderService.OpenFolderDialog())
            {
                InvalidRepositoryOpenReasons reason = InvalidRepositoryOpenReasons.None;

                reason = Repository.CanOpenRepository(openFolderService.FilePath, App.User.Id);
                switch (reason)
                {
                    case InvalidRepositoryOpenReasons.None:
                        Repository.OpenLocal(openFolderService.FilePath, App.User.Id);
                        dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "Вы успешно отркцли репозиторий!");
                        windowsService.NavigateToWindow(new RepositoryWindow());
                        break;
                    case InvalidRepositoryOpenReasons.FolderDoesNotContainRepository:
                        dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "Невозможно найти файл инициализации в  : \n" +
                            openFolderService.FilePath);
                        break;
                    case InvalidRepositoryOpenReasons.RepositoryDoNotBelongToUser:
                        dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "Репозиторий принадлежит другому пользователю");
                        break;
                    default:
                        break;
                }
            };
        }

        private async void Download(object obj)
        {
            string message = string.Empty;
            bool rc = false;

            if (openFolderService.OpenFolderDialog())
            {
                InvalidRepositoryDownloadReasons reason = InvalidRepositoryDownloadReasons.None;
                reason = Repository.CanDownloadRepository(openFolderService.FilePath + "\\" + SelectedRepository.Title);

                switch (reason)
                {
                    case InvalidRepositoryDownloadReasons.None:
                        rc = Repository.Download(openFolderService.FilePath, SelectedRepository.Id.ToString(), out message);
                        bool response = await dialogService.ShowMessageDialog(
                            windowsService.CurrentOpenedWindow, "Вы хотите открыть репозиторий?");
                        if(response)
                        {
                            windowsService.NavigateToWindow(new RepositoryWindow());

                        }
                        break;
                    case InvalidRepositoryDownloadReasons.FolderAlreadyContainsRepository:
                        if (await dialogService.ShowMessageDialog(windowsService.CurrentOpenedWindow,
                             "По данному путю уже существует каталог со схожим именем, вы зотите его перезаписать :" +
                            openFolderService.FilePath + " ?"))
                        {
                            rc = Repository.Download(openFolderService.FilePath, SelectedRepository.Id.ToString(), out message);
                        }
                        break;
                    default:
                        break;
                }
                if (!rc)
                {
                    dialogService.ShowMessage(windowsService.CurrentOpenedWindow, message);
                }
            }
        }

        private void Delete(object obj)
        {
            LfkClient.Repository.Repository.GetInstance().Delete(SelectedRepository.Id.ToString());
        }

        private async void Create(object obj)
        {
            LocalRepository.Id = Guid.NewGuid();
            LocalRepository.UserId = App.User.Id;

            InvalidRepositoryCreationReasons reason = InvalidRepositoryCreationReasons.None;

            reason = Repository.CanCreateRepository(LocalRepository);
            switch (reason)
            {
                case InvalidRepositoryCreationReasons.None:
                    CreateRepository(LocalRepository);
                    break;
                case InvalidRepositoryCreationReasons.DuplicateTitle:
                    dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "У вас уже есть репозиторий с именем " + LocalRepository.Title);
                    break;
                case InvalidRepositoryCreationReasons.FolderAlreadyContainsRepository:
                    if (await dialogService.ShowMessageDialog(
                        windowsService.CurrentOpenedWindow,
                        "Вы уверены что хотите перезаписать репозиторий в :" +
                        LocalRepository.Path + " ?"))
                    {
                        CreateRepository(LocalRepository);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        private async void CreateRepository(LocalRepository localRepository)
        {
            string message;
            if (Repository.TryInit(localRepository, out message))
            {
                if (await dialogService.ShowMessageDialog(
                    windowsService.CurrentOpenedWindow
                    , "Вы хотите открыть репозиторий?"))
                {
                    windowsService.NavigateToWindow(new RepositoryWindow());
                }
            }
            else
            {
                dialogService.ShowMessage(windowsService.CurrentOpenedWindow, message);
            }
        }
    }

}

