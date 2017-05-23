using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using LfkSharedResources.Models.Repository;
using LfkSharedResources.Models.User;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using LfkExceptions;
using LfkClient.UserMessages;

namespace LfkGUI.RepositoryManagement
{
    /// <summary>
    /// Логика взаимодействия для RepositoryManagementWindow.xaml
    /// </summary>
    public partial class RepositoryManagementWindow : Base.BaseWindow
    {
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();
        public RepositoryManagementWindow()
        {
            InitializeComponent();
        }
        #region Главные обработчики

        private async void OpenLocalRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            string path;
            if (GetFolderBrowseDialogSelectedPath(out path))
            {
                InvalidRepositoryOpenReasons reason = InvalidRepositoryOpenReasons.None;

                reason = Repository.CanOpenRepository(path, App.User.Id);
                switch (reason)
                {
                    case InvalidRepositoryOpenReasons.None:
                        Repository.OpenLocal(path, App.User.Id);
                        await this.ShowMessageAsync("You open repository!", "Let's start working with it",
                                 MessageDialogStyle.Affirmative);
                        this.Closing += OnOpenRepository;
                        this.Close();
                        break;
                    case InvalidRepositoryOpenReasons.FolderDoesNotContainRepository:
                        await this.ShowMessageAsync("ERROR!", "Can't find initialization file in : \n" + path,
                                 MessageDialogStyle.Affirmative);
                        break;
                    case InvalidRepositoryOpenReasons.RepositoryDoNotBelongToUser:
                        await this.ShowMessageAsync("ERROR!", "Repository does not belong to current user",
                                MessageDialogStyle.Affirmative);
                        break;
                    default:
                        break;
                }
            };
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            List<LocalRepository> repositories =
                Repository.GetManagedRepositories(App.User.Id.ToString());
            UserRepositoriesListView.ItemsSource = repositories;
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            LocalRepository lr = (UserRepositoriesListView.SelectedItem as LocalRepository);
            string message = string.Empty;
            bool rc = false;
            if (lr != null)
            {
                string path;
                if (GetFolderBrowseDialogSelectedPath(out path))
                {
                    InvalidRepositoryDownloadReasons reason = InvalidRepositoryDownloadReasons.None;
                    MessageDialogResult result;
                    reason = Repository.CanDownloadRepository(path);
                    switch (reason)
                    {
                        case InvalidRepositoryDownloadReasons.None:
                            rc = Repository.Download(path, lr.Id.ToString(), out message);
                            result =  await this.ShowMessageAsync("You download repository!",
                                "Do you want to open it?",
                                MessageDialogStyle.AffirmativeAndNegative);
                            if (result == MessageDialogResult.Affirmative)
                            {
                                this.Closing += OnOpenRepository;
                                this.Close();
                            }
                            break;
                        case InvalidRepositoryDownloadReasons.FolderAlreadyContainsRepository:
                            result = await this.ShowMessageAsync("Внимание",
                                "Вы уверены что хотите перезаписать репозиторий в :" +
                                path + " ?",
                                MessageDialogStyle.AffirmativeAndNegative);
                            if (result == MessageDialogResult.Affirmative)
                            {
                                rc = Repository.Download(path, lr.Id.ToString(), out message);
                            }
                            break;
                        default:
                            break;
                    }
                    if (!rc)
                    {
                        await this.ShowMessageAsync("Ошибка", message,
                       MessageDialogStyle.Affirmative);
                    }
                }
            }
            else
            {
                await this.ShowMessageAsync("Внимание", "Пожалуйста выберите репозиторий для скачивания",
                       MessageDialogStyle.Affirmative);
            }
        }

        private async void CreateRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            LocalRepository localRepository = new LocalRepository
            {
                Id = Guid.NewGuid(),
                Title = RepositoryTitle.Text,
                Path = RepositoryFolderPath.Text,
                UserId = App.User.Id
            };
            InvalidRepositoryCreationReasons reason = InvalidRepositoryCreationReasons.None;

            reason = Repository.CanCreateRepository(localRepository);
            switch (reason)
            {
                case InvalidRepositoryCreationReasons.None:
                    CreateRepository(localRepository);
                    break;
                case InvalidRepositoryCreationReasons.DuplicateTitle:
                    await this.ShowMessageAsync("Ошибка", "У вас уже есть репозиторий с именем " + localRepository.Title,
                        MessageDialogStyle.Affirmative);
                    break;
                case InvalidRepositoryCreationReasons.FolderAlreadyContainsRepository:
                    MessageDialogResult result = await this.ShowMessageAsync("Внимание",
                        "Вы уверены что хотите перезаписать репозиторий в :" +
                        localRepository.Path + " ?",
                      MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        CreateRepository(localRepository);
                    }
                    break;
                default:
                    break;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            LocalRepository lr = (UserRepositoriesListView.SelectedItem as LocalRepository);
            if (lr != null)
            {
                LfkClient.Repository.Repository.GetInstance().Delete(lr.Id.ToString());
            }
        }
        #endregion

        #region Вспомогательные методы
        private void OnOpenRepository(object sender, EventArgs e)
        {
            Repository.RepositoryWindow rw = new Repository.RepositoryWindow();
            rw.Show();
        }

        private void RepositoryFolderPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string path;
            if (GetFolderBrowseDialogSelectedPath(out path))
            {
                RepositoryFolderPath.Text = path;
            };
        }

        private async void CreateRepository(LocalRepository localRepository)
        {
            string message;
            if (Repository.TryInit(localRepository, out message))
            {
                MessageDialogResult result = await this.ShowMessageAsync("Внимание", "Вы хотите открыть репозиторий?",
                  MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    this.Closing += OnOpenRepository;
                    this.Close();
                }
            }
            else
            {
                MessageDialogResult result = await this.ShowMessageAsync("Ошибка", message,
                         MessageDialogStyle.Affirmative);
            }
        }

        private void ShowCreateRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            RepositoryCreationMenu.Visibility =
                RepositoryCreationMenu.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool GetFolderBrowseDialogSelectedPath(out string selectedPath)
        {
            bool rc = false;
            selectedPath = string.Empty;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = fbd.SelectedPath;
                rc = true;
            }
            return rc;
        }
        #endregion
    }
}