using System;
using System.Windows;
using System.Windows.Controls;
using LfkClient.Authorization;
using LfkSharedResources.Models.User;
using MahApps.Metro.Controls.Dialogs;
using LfkGUI.Utility.Validation;

using System.Threading.Tasks;
using LfkGUI.ViewModels.AuthorizationViewModels;
using LfkGUI.Services;

namespace LfkGUI.Views.AuthorizationViews
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Base.BaseWindow
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            AuthorizationFrame.Content = Resources["WelcomeTextBlock"];
        }

        private void OnSuccessAuthorization(object sender, EventArgs e)
        {
            //RepositoryManagementWindow rmw = new RepositoryManagementWindow();
            //rmw.Show();
        }

        private void SignupShowButton_Click(object sender, RoutedEventArgs e)
        {
            SignupPage signupPage = new SignupPage();
            signupPage.DataContext = new SignupViewModel(new SignupUser() { Email = "example@mail.com", Name = "Vanya" }, new DialogService(), new WindowsService(this));
            AuthorizationFrame.Content = signupPage;
        }

        private void LoginShowButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.DataContext = new LoginViewModel(new LoginUser() { Email = "example@mail.com", Password = "example" }, new DialogService(), new WindowsService(this));
            AuthorizationFrame.Content = loginPage;
        }
    }
}