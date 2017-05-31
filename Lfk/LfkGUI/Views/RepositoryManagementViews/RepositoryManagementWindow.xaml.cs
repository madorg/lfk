using System.Windows;
using LfkGUI.ViewModels.RepositoryManagementViewModels;
using LfkGUI.Services;

namespace LfkGUI.Views.RepositoryManagementViews
{
    /// <summary>
    /// Логика взаимодействия для RepositoryManagementWindow.xaml
    /// </summary>
    public partial class RepositoryManagementWindow : Base.BaseWindow
    {
        public RepositoryManagementWindow()
        {
            InitializeComponent();
            this.DataContext = new RepositoryManagementViewModel(new WindowsService(this));
        }

        private void ShowCreateRepositoryButton_Click(object sender, RoutedEventArgs e)
        {
            RepositoryCreationMenu.Visibility =
                RepositoryCreationMenu.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}