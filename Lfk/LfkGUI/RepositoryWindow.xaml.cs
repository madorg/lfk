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
using LfkClient.Repository;
using LfkClient.Models.Repository;
using LfkClient.Models.User;
using LfkClient.Models;

namespace LfkGUI
{
    /// <summary>
    /// Interaction logic for RepositoryWindow.xaml
    /// </summary>
    public partial class RepositoryWindow : Window
    {
        public RepositoryWindow()
        {
            InitializeComponent();

            string tempPath = @"F:\lfk_tests";
            Repository.GetInstance().Init(new LocalRepository()
            {
                Id = Guid.NewGuid(),
                Title = tempPath.Split('\\').Last(),
                UserId = (App.Current.Resources["AppUser"] as User).Id,
                Path = tempPath
            });

            foreach (string file in Repository.GetInstance().GetWorkingDirectoryFiles())
            {
                WorkingDirectoryTextBox.Text += file + '\n';
            }
        }

        private void IncludeCommandMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddCommandMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IncludeButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> includedFiles = new List<string>(IncludeTextBox.Text.Split('\n'));
            Repository.GetInstance().Include(includedFiles);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string fileToAdd = AddTextBox.Text;
            List<string> addedFiles = new List<string>(AddTextBox.Text.Split('\n'));
            Repository.GetInstance().Add(addedFiles);
        }

        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            Repository.GetInstance().Commit(CommitMessageTextBox.Text);
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            List<Commit> commits = Repository.GetInstance().History();

            foreach (Commit commit in commits)
            {
                CommitsComboBox.Items.Add(commit.Id);
            }
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            Repository.GetInstance().Switch(CommitsComboBox.SelectedItem.ToString());
        }
    }
}