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
    public partial class RepositoryWindow : MetroWindow
    {
        
        public RepositoryWindow()
        {
            InitializeComponent();
            App.Current.Resources["AppUser"] = new User();

            string tempPath = @"D:\lfk_tests";
            LfkClient.Repository.Repository.GetInstance().Init(new LocalRepository()
            {
                Id = Guid.NewGuid(),
                Title = tempPath.Split('\\').Last(),
                UserId = (App.Current.Resources["AppUser"] as User).Id,
                Path = tempPath
            });
        }

        private TreeViewItem BuildFilesTreeViewItem(string[] filenames)
        {
            TreeViewItem root = new TreeViewItem();
            //TODO НАПИСАТЬ ФУНКЦИЮ ПрЕОБРАЗОВАНИЯ СТРОК В ДРЕВОВИДНУЮ СТРУКТУРУ
            return root;
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
    }
}