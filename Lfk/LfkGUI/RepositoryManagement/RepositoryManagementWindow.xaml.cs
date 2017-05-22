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

namespace LfkGUI.RepositoryManagement
{
    /// <summary>
    /// Логика взаимодействия для RepositoryManagementWindow.xaml
    /// </summary>
    public partial class RepositoryManagementWindow : Base.BaseWindow
    {
        public RepositoryManagementWindow()
        {
            InitializeComponent();
        }

        private void OnOpenRepository(object sender, EventArgs e)
        {
            LfkGUI.Repository.RepositoryWindow rw = new LfkGUI.Repository.RepositoryWindow();
            rw.Show();
        }
        private async void OpenLocalRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LfkClient.Repository.Repository.GetInstance().OpenLocal(
                        fbd.SelectedPath,App.User.Id);
                    MessageDialogResult result = await this.ShowMessageAsync("You open repository!", "Let's start working with it",
                             MessageDialogStyle.Affirmative);

                    this.Closing += OnOpenRepository;
                    this.Close();
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    MessageDialogResult result = await this.ShowMessageAsync("ERROR!", "Can't find initialization file: \n" + ex.Message,
                             MessageDialogStyle.Affirmative);
                }
                catch(NotAllowedOpenRepository naop)
                {
                    MessageDialogResult result = await this.ShowMessageAsync("ERROR!", naop.Message,
                             MessageDialogStyle.Affirmative);
                }

            }
        }

        private async void CreateRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            LocalRepository localRepository = null;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string message;

                LfkClient.Repository.Repository repo = LfkClient.Repository.Repository.GetInstance();
                localRepository = new LocalRepository()
                {
                    Id = Guid.NewGuid(),
                    Title = fbd.SelectedPath.Split('\\').Last(),
                    UserId = App.User.Id,
                    Path = fbd.SelectedPath
                };
                bool created = repo.TryInit(localRepository, out message);

                if (created)
                {
                    MessageDialogResult result = await this.ShowMessageAsync("New repository!", "Do you want to open it?",
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
                         MessageDialogStyle.AffirmativeAndNegative);
                    if(result == MessageDialogResult.Negative)
                    {
                        LfkClient.Repository.Repository.GetInstance().Delete(localRepository.Id.ToString());
                    }
                }
            }
        }

        private async void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<LocalRepository> repositories =
                    LfkClient.Repository.Repository.GetInstance().GetManagedRepositories(App.User.Id.ToString());
                UserRepositoriesListView.ItemsSource = repositories;
            }
            catch (Exception ex)
            {
                MessageDialogResult result = await this.ShowMessageAsync("Ошибка", ex.Message,
                        MessageDialogStyle.Affirmative);
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

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            LocalRepository lr = (UserRepositoriesListView.SelectedItem as LocalRepository);
            string message;
            if (lr != null)
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    bool downloaded =
                        LfkClient.Repository.Repository.GetInstance().Download(fbd.SelectedPath, lr.Id.ToString(), out message);

                    if (downloaded)
                    {
                        MessageDialogResult result = await this.ShowMessageAsync(
                            "New repository!",
                            "Do you want to open it?",
                        MessageDialogStyle.AffirmativeAndNegative);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            this.Closing += OnOpenRepository;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageDialogResult result = await this.ShowMessageAsync(
                            "Ошибка",
                            message,
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

    }
}