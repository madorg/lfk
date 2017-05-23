using System;
using System.Windows;
using System.Windows.Controls;
using LfkClient.Authorization;
using LfkSharedResources.Models.User;
using LfkGUI.RepositoryManagement;
using MahApps.Metro.Controls.Dialogs;
using LfkGUI.Utility.Validation;
using System.Threading.Tasks;
namespace LfkGUI.Authorization
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
            RepositoryManagementWindow rmw = new RepositoryManagementWindow();
            rmw.Show();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = AuthorizationFrame.Content as LoginPage;
            string message = "Проверьте правильность введенных данных";

            if (loginPage != null)
            {
                LoginUser loginUser = new LoginUser()
                {
                    Email = loginPage.LoginEmailTextBox.Text,
                    Password = loginPage.LoginPasswordTextBox.Password
                };
                Guid userId = Guid.Empty;
                var controller =  await this.ShowProgressAsync("Подождите", "Сервер слишком далеко");
                
                bool rc = await Task.Run(() =>
                {
                   return Authorizator.TryLogin(loginUser, out message, out userId);
                });
                if (rc)
                {
                    App.User = new User()
                    {
                        Id = userId
                    };
                    await controller.CloseAsync();
                    this.Closing += OnSuccessAuthorization;
                    this.Close();
                }
                else
                {
                    MessageDialogResult result = await this.ShowMessageAsync("Attention!", message,
                        MessageDialogStyle.AffirmativeAndNegative);
                }
            }
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationPage = AuthorizationFrame.Content as RegistrationPage;

            if (registrationPage != null)
            {
                string message = "Неверные данные";
                Guid userId;
                SignupUser signupUser = new SignupUser()
                {
                    Name = registrationPage.SignupNameTextBox.Text,
                    Email = registrationPage.SignupEmailTextBox.Text,
                    Password = registrationPage.SignupPasswordTextBox.Password
                };
                if (registrationPage.SignupPasswordTextBox.Password == registrationPage.SignupConfirmPasswordTextBox.Password)
                {
                    if (SignupValidation.IsValid(signupUser) && Authorizator.TrySignup(signupUser, out message, out userId))
                    {
                        MessageDialogResult result = await this.ShowMessageAsync("You have signed in successfully", message,
                            MessageDialogStyle.Affirmative);

                        App.User = new User()
                        {
                            Id = userId

                            // ОСТАЮТСЯ ПУСТЫЕ ПОЛЯ (???)
                        };

                        this.Closing += OnSuccessAuthorization;
                        this.Close();
                    }
                    else
                    {
                        MessageDialogResult result = await this.ShowMessageAsync("Error", message,
                           MessageDialogStyle.Affirmative);

                        AuthorizationFrame.Content = new RegistrationPage();
                        SetActionButton("SignupButton");
                    }
                }
                else
                {
                    MessageDialogResult result = await this.ShowMessageAsync("Error", "Пароли не совпадают",
                           MessageDialogStyle.Affirmative);
                }
            }
        }

        private void SignupShowButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationFrame.Content = new RegistrationPage();
            SetActionButton("SignupButton");
        }

        private void LoginShowButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationFrame.Content = new LoginPage();
            SetActionButton("LoginButton");
        }

        private void SetActionButton(string buttonKey)
        {
            ActionButtonStackPanel.Children.Clear();
            ActionButtonStackPanel.Children.Add(Resources[buttonKey] as Button);
        }
    }
}