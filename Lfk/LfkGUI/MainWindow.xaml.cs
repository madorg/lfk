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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LfkClient.Models.User;
using LfkClient.Authorization;
using LfkClient.Repository;

namespace LfkGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LfkClient.Repository.Repository repo = new LfkClient.Repository.Repository();

        public MainWindow()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Authorizator.TryLogin(new LoginUser() { username = Username.Text, password = Password.Text });
            InitButton.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //repo.Init(new LfkClient.Models.Repository.LocalRepository() { Id = 1, Title = "First repo", UserId = , Path = @"F:\garbage\lfk" });

            // TODO: Добавить путь к репозиторию!!!
        }

        private void IncludeButton_Click(object sender, RoutedEventArgs e)
        {
            //repo.Include(new List<string>() { "myfile.txt", "mydoc.docx" });
        }
    }
}