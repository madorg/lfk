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
using System.Windows.Forms;
using LfkClient.Models.Repository;
using LfkClient.Repository;
using LfkClient.Models.User;

namespace LfkGUI
{
    /// <summary>
    /// Interaction logic for RepositoryWindow.xaml
    /// </summary>
    public partial class RepositoryManagementWindow : Window
    {
        public RepositoryManagementWindow()
        {
            InitializeComponent();

            this.Closing += OnOpenRepository;
        }

        private void OnOpenRepository(object sender, EventArgs e)
        {
            RepositoryWindow rw = new RepositoryWindow();
            rw.Show();
        }

        private void CreateRepositoryTEST()
        {
            Repository repo = Repository.GetInstance();

            string tempPath = @"F:\lfk_tests";

            repo.Init(new LocalRepository()
            {
                Id = Guid.NewGuid(),
                Title = tempPath.Split('\\').Last(),
                UserId = (App.Current.Resources["AppUser"] as User).Id,
                Path = tempPath
            });

            this.Close();
        }

        private void CreateRepositoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;


            CreateRepositoryTEST();

            //if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    Repository repo = Repository.GetInstance();

            //    string tempPath = @"F:\lfk_tests";

            //    repo.Init(new LocalRepository()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = fbd.SelectedPath.Split('\\').Last(),
            //        UserId = (App.Current.Resources["AppUser"] as User).Id,
            //        Path = fbd.SelectedPath
            //    });

            //    this.Close();
            //}
        }

        private void OpenRepositoryFromFolderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}