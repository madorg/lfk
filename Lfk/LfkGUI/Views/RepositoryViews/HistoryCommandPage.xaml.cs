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
using LfkGUI.ViewModels.RepositoryViewModels;
using LfkGUI.Services;

namespace LfkGUI.Views.RepositoryViews
{
    /// <summary>
    /// Логика взаимодействия для HistoryCommandPage.xaml
    /// </summary>
    public partial class HistoryCommandPage : Page
    {
        public HistoryCommandPage()
        {
            InitializeComponent();
            this.DataContext = new RepositoryHistoryCommandViewModel(new WindowsService(App.Current.MainWindow));
        }

    }
}