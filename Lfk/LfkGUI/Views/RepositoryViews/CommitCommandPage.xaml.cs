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
using LfkGUI.ViewModels.RepositoryViewModels;
using LfkGUI.Services;

namespace LfkGUI.Views.RepositoryViews
{
    /// <summary>
    /// Логика взаимодействия для CommitCommandPage.xaml
    /// </summary>
    public partial class CommitCommandPage : Page
    {
        public CommitCommandPage()
        {
            InitializeComponent();
            this.DataContext = new RepositoryCommitCommandViewModel(new WindowsService(App.Current.MainWindow));
        }
    }
}