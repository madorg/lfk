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
using LfkExceptions;
using MahApps.Metro.Controls.Dialogs;
using LfkGUI.ViewModels.RepositoryViewModels;
using LfkGUI.Services;

namespace LfkGUI.Views.RepositoryViews
{
    /// <summary>
    /// Логика взаимодействия для RepositoryWindow.xaml
    /// </summary>
    public partial class RepositoryWindow : Base.BaseWindow
    {
        
        public RepositoryWindow()
        {
            InitializeComponent();
            string repositoryPath = LfkClient.Repository.Repository.GetInstance().GetCurrentRepositoryName();
            this.Title = " [ " + repositoryPath + " ] ";
            this.DataContext = new RepositoryViewModel(new WindowsService(this));

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
            //new RepositoryManagement.RepositoryManagementWindow().Show();
            //this.Close();
        }


        private async void UpdateCommandButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LfkClient.Repository.Repository.GetInstance().Update();
            }
            catch (RepositoryUpdateWithoutCommitsException ruwce)
            {
                await this.ShowMessageAsync("Ошибка", ruwce.Message,
                             MessageDialogStyle.Affirmative);
            }
        }
    }
}