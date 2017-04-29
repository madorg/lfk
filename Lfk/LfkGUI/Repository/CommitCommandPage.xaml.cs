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

namespace LfkGUI.Repository
{
    /// <summary>
    /// Логика взаимодействия для CommitCommandPage.xaml
    /// </summary>
    public partial class CommitCommandPage : Page
    {
        public CommitCommandPage()
        {
            InitializeComponent();
        }

        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            LfkClient.Repository.Repository.GetInstance().Commit(CommitMessageTextBox.Text);
        }
    }
}
