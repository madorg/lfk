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
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
namespace LfkGUI.Repository
{
    /// <summary>
    /// Логика взаимодействия для CommitCommandPage.xaml
    /// </summary>
    public partial class CommitCommandPage : Page
    {
        private void UpdateChangedFilesListBox()
        {
            LastChangedFilesListBox.Items.Clear();
            foreach (var item in LfkClient.Repository.Repository.GetInstance().GetChangedFilesAfterLastCommit())
            {
                LastChangedFilesListBox.Items.Add(new ListBoxItem()
                {
                    Content = item,
                    Foreground = Brushes.SpringGreen
                });
            }
        }
        public CommitCommandPage()
        {
            InitializeComponent();
            UpdateChangedFilesListBox();

        }

        private async void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CommitMessageTextBox.Text)) { 
                LfkClient.Repository.Repository.GetInstance().Commit(CommitMessageTextBox.Text);
                UpdateChangedFilesListBox();
                CommitMessageTextBox.Clear();
            }
            else if(LastChangedFilesListBox.Items.Count == 0)
            {
                DockPanel modalDialog = App.Current.Resources["ModalDialogWithText"] as DockPanel;
                (modalDialog.Children[0] as TextBlock).Text = "Error!";
                (modalDialog.Children[1] as TextBlock).Text = "No changes to make commit!";
                DialogContent.Children.Add(modalDialog);
                await DialogHost.Show(DialogContent);
                DialogContent.Children.Remove(modalDialog);

            }
            else
            {
                DockPanel modalDialog = App.Current.Resources["ModalDialogWithText"] as DockPanel;
                (modalDialog.Children[0] as TextBlock).Text = "Error!";
                (modalDialog.Children[1] as TextBlock).Text = "Please insert commit message to make commit!";
                DialogContent.Children.Add(modalDialog);
                await DialogHost.Show(DialogContent);
                DialogContent.Children.Remove(modalDialog);
            }
        }
    }
}
