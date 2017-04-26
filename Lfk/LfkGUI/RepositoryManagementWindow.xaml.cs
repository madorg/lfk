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
        }

        private void CreateRepositoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Repository repo = new Repository();

                repo.Init(new LocalRepository()
                {
                    Id = Guid.NewGuid(),
                    Title = fbd.SelectedPath.Split('\\').Last(),
                    UserId = (App.Current.Resources["AppUser"] as User).Id,
                    Path = fbd.SelectedPath
                });
            }
        }
    }
}