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
            (HistoryListView.Resources["Commits"] as ArrayList).AddRange(
                LfkClient.Repository.Repository.GetInstance().History()
                );
        }

        private void SwitchCommandButton_Click(object sender, RoutedEventArgs e)
        {
            //LfkClient.Repository.Repository.GetInstance().Switch(CommitsComboBox.SelectedItem.ToString());
        }
    }
}
