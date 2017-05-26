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
using System.Collections;
using MaterialDesignThemes.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using LfkClient.UserMessages;

namespace LfkGUI.Repository
{
    /// <summary>
    /// Логика взаимодействия для HistoryCommandPage.xaml
    /// </summary>
    public partial class HistoryCommandPage : Page
    {
        public HistoryCommandPage()
        {
            InitializeComponent();
            var history = LfkClient.Repository.Repository.GetInstance().History();
            if (history != null && history.Count != 0)
            {
                (HistoryListView.Resources["Commits"] as ArrayList).AddRange(
                    history
                    );
            }
        }

        private async void SwitchCommandButton_Click(object sender, RoutedEventArgs e)
        {
            MetroWindow window = App.Current.MainWindow as MetroWindow;
            LfkSharedResources.Models.Commit commit = HistoryListView.SelectedItem as LfkSharedResources.Models.Commit;

            InvalidCommitSwitchingReasons reason = LfkClient.Repository.Repository.GetInstance().CanSwitch(commit);

            switch (reason)
            {
                case InvalidCommitSwitchingReasons.None:
                    SwitchCommit(commit);
                    break;

                case InvalidCommitSwitchingReasons.NotCommittedChanges:
                    MessageDialogResult result = await window.ShowMessageAsync("Bad", "Вы уверены, что хотите переключиться на другой коммит без сохранения текущего состояния? \n", MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        SwitchCommit(commit);
                    }
                    break;

                default:
                    break;
            }
        }

        private async void SwitchCommit(LfkSharedResources.Models.Commit commit)
        {
            MetroWindow window = App.Current.MainWindow as MetroWindow;
            LfkClient.Repository.Repository.GetInstance().Switch(commit);
            await window.ShowMessageAsync("Success", "Успешное переключение на коммит : \n" +
                commit.Id.ToString() +
                "\n" + "Сообщение : " +
                commit.Comment, MessageDialogStyle.Affirmative, new MetroDialogSettings() { ColorScheme = MetroDialogColorScheme.Inverted });
        }

        private async void HistoryListView_Selected(object sender, RoutedEventArgs e)
        {
            //LfkSharedResources.Models.Commit selectedCommit = HistoryListView.SelectedItem as LfkSharedResources.Models.Commit;


            //ContentText.Text = "";
            //foreach (var item in selectedCommit.Index.RepoObjectIdAndFileName)
            //{
            //    ContentText.Text += item.Value + "\n";
            //}
            //await DialogHost.Show(DialogContent);
        }

    }
}