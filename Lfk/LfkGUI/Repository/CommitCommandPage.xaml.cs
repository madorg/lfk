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
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

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
            MetroWindow mainWindow = Application.Current.MainWindow as MetroWindow;
            if (mainWindow != null)
            {
                if (LastChangedFilesListBox.Items.Count == 0)
                {
                    await mainWindow.ShowMessageAsync("Ошибка", "Нечего коммитить",
                                 MessageDialogStyle.Affirmative);
                }
                else if (!string.IsNullOrWhiteSpace(CommitMessageTextBox.Text))
                {
                    LfkClient.Repository.Repository.GetInstance().Commit(CommitMessageTextBox.Text);
                    UpdateChangedFilesListBox();
                    CommitMessageTextBox.Clear();
                }
                else
                {
                    await mainWindow.ShowMessageAsync("Ошибка", "Введите сообщение коммита",
                                  MessageDialogStyle.Affirmative);
                }
            }
        }
    }
}