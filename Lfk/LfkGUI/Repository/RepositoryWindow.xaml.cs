﻿using System;
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
using LfkClient.Models.User;
using LfkClient.Models.Repository;
using LfkClient.Models;
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

            string tempPath = @"F:\book";
            LfkClient.Repository.Repository.GetInstance().Init(new LocalRepository()
            {
                Id = Guid.NewGuid(),
                Title = tempPath.Split('\\').Last(),
                UserId = (App.Current.Resources["AppUser"] as User).Id,
                Path = tempPath
            });

            //LfkClient.Repository.Repository.GetInstance().GetChangedFiles();
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
            RepositoryManagement.RepositoryManagementWindow rmw = new RepositoryManagement.RepositoryManagementWindow();
            rmw.Show();
            this.Close();
        }

    }
}