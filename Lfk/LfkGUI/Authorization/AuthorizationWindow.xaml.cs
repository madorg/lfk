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
using System.Windows.Shapes;
using LfkClient.Authorization;
using LfkSharedResources.Models.User;
using System.ComponentModel.DataAnnotations;
using MahApps.Metro.Controls;
using LfkGUI.RepositoryManagement;
using LfkGUI.Utility.Validation;
using LfkSharedResources.Serialization.Json;
using LfkClient.FileSystemControl;

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
            // TODO: добавить параметры с информаицей о юзере

            RepositoryManagementWindow rmw = new RepositoryManagementWindow();
            rmw.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = AuthorizationFrame.Content as LoginPage;
            if (loginPage != null)
            {
                LoginUser loginUser = new LoginUser()
                {
                    Email = loginPage.LoginEmailTextBox.Text,
                    Password = loginPage.LoginPasswordTextBox.Password
                };

                //JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
                //JsonSerializer.WriteMethod = FileSystem.WriteToFile;

                string message;
                Guid userId;

                if (Authorizator.TryLogin(loginUser, out message, out userId))
                {
                    MessageBox.Show(message);

                    App.User = new User()
                    {
                        Id = userId
                    };

                    this.Closing += OnSuccessAuthorization;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationPage = AuthorizationFrame.Content as RegistrationPage;

            if (registrationPage != null)
            {
                string message;
                Guid userId;

                bool rc = Authorizator.TrySignup(new SignupUser()
                {
                    Name = registrationPage.SignupNameTextBox.Text,
                    Email = registrationPage.SignupEmailTextBox.Text,
                    Password = registrationPage.SignupPasswordTextBox.Password
                }, out message, out userId);

                if (rc)
                {
                    MessageBox.Show(message);

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
                    MessageBox.Show(message);

                    AuthorizationFrame.Content = new RegistrationPage();
                    SetActionButton("SignupButton");
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