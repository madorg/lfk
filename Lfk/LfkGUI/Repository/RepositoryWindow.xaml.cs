using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using LfkSharedResources.Models.User;
using LfkSharedResources.Models.Repository;
using LfkSharedResources.Models;
using MahApps.Metro.Controls;

namespace LfkGUI.Repository
{
    /// <summary>
    /// Логика взаимодействия для RepositoryWindow.xaml
    /// </summary>
    public partial class RepositoryWindow : Base.BaseWindow
    {
        
        public RepositoryWindow()
        {
            InitializeComponent();
            App.Current.Resources["AppUser"] = new User();

            //string tempPath = @"F:\lfk_tests";
            //LfkClient.Repository.Repository.GetInstance().TryInit(new LocalRepository()
            //{
            //    Id = Guid.NewGuid(),
            //    Title = tempPath.Split('\\').Last(),
            //    UserId = (App.Current.Resources["AppUser"] as User).Id,
            //    Path = tempPath
            //});
        }
        public RepositoryWindow(string s)
        {
            InitializeComponent();
        }
        private void IncludeCommandButton_Click(object sender, RoutedEventArgs e)
        {
            CommandFrame.Content = new IncludeCommandPage();
        }

        private void AddCommandButton_Click(object sender, RoutedEventArgs e)
        {
            CommandFrame.Content = new AddCommandPage();
        }

        private void CommitCommandButton_Click(object sender, RoutedEventArgs e)
        {
            CommandFrame.Content = new CommitCommandPage();
        }

        private void HistoryCommandButton_Click(object sender, RoutedEventArgs e)
        {
            CommandFrame.Content = new HistoryCommandPage();
        }

        private void NavigateToRepositoryManagementWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new RepositoryManagement.RepositoryManagementWindow().Show();
            this.Close();
        }

        private void UploadCommandButton_Click(object sender, RoutedEventArgs e)
        {
            LfkClient.Repository.Repository.GetInstance().Upload();
        }
    }
}