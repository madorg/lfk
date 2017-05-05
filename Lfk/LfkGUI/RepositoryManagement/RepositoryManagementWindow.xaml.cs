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
using LfkClient.Models.Repository;
using LfkClient.Models.User;
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
                try
                {
                    LfkClient.Repository.Repository repo = LfkClient.Repository.Repository.GetInstance();
                    repo.Init(new LocalRepository()
                    {
                        Id = Guid.NewGuid(),
                        Title = fbd.SelectedPath.Split('\\').Last(),
                        UserId = (App.Current.Resources["AppUser"] as User).Id,
                        Path = fbd.SelectedPath
                    });
                    MessageDialogResult result = await this.ShowMessageAsync("New repository!", "Do you want to open it?",
                             MessageDialogStyle.AffirmativeAndNegative);

                    if(result == MessageDialogResult.Affirmative)
                    {
                        this.Closing += OnOpenRepository;
                        this.Close();
                    }

                }
                catch
                {
                    throw;
                }

            }

        }
    }
}
