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
using LfkGUI.Repository;

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
            RepositoryWindow rw = new RepositoryWindow("");
            rw.Show();
        }

        private void OpenRepositoryDropDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (!MenuStackPanel.Children.Contains(Resources["OpenLocalRepositoryButton"] as Button) ||
               !MenuStackPanel.Children.Contains(Resources["OpenRemoteRepositoryButton"] as Button))
            {
                MenuStackPanel.Children.Insert(2, Resources["OpenLocalRepositoryButton"] as Button);
                MenuStackPanel.Children.Insert(3, Resources["OpenRemoteRepositoryButton"] as Button);
            }
            else
            {
                MenuStackPanel.Children.RemoveRange(2, 2);
            }
        }

        private async void OpenLocalRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog() {
                ShowNewFolderButton = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };
           
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LfkClient.Repository.Repository.GetInstance().OpenLocal(
                        fbd.SelectedPath);
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

            }
        }

        private void OpenRemoteRepositoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void CreateRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string message;

                LfkClient.Repository.Repository repo = LfkClient.Repository.Repository.GetInstance();
                bool created = repo.TryInit(new LocalRepository()
                {
                    Id = Guid.NewGuid(),
                    Title = fbd.SelectedPath.Split('\\').Last(),
                    UserId = App.User.Id,
                    Path = fbd.SelectedPath
                }, out message);

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
                    MessageBox.Show(message, "Ошибка при создании репозитория", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            List<LocalRepository> repositories = LfkClient.Repository.Repository.GetInstance().GetManagedRepositories(App.User.Id.ToString());
            repositories.ForEach(r => System.Windows.Forms.MessageBox.Show(r.Title));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            LfkClient.Repository.Repository.GetInstance().Download("f8aa4309-7517-4c80-9eb7-49f9a8364404");
        }
    }
}