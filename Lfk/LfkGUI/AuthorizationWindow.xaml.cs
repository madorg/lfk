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
using LfkClient.Models.User;

namespace LfkGUI
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            this.SignupForm.Visibility = Visibility.Hidden;
        }

        private void OnSuccessAuthorization(object sender, EventArgs e)
        {
            // TODO: добавить параметры с информаицей о юзере



            RepositoryManagementWindow rmw = new RepositoryManagementWindow();
            rmw.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: check parms

            bool rc = Authorizator.TryLogin(new LoginUser() { username = LoginUsernameTextBox.Text, password = LoginPasswordTextBox.Text });

            if (rc)
            {

                this.Closing += OnSuccessAuthorization;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный ввод!");
            }
        }

        private void TrySignupButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainAuthorizationForm.Visibility = Visibility.Hidden;
            this.SignupForm.Visibility = Visibility.Visible;
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            bool rc = Authorizator.TrySignup(new SignupUser()
            {
                Name = SignupNameTextBox.Text,
                Email = SignupEmailTextBox.Text,
                Password = SignupPasswordTextBox.Text
            });

            if (rc)
            {
                App.Current.Resources["AppUser"] = new User();
                this.Closing += OnSuccessAuthorization;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный ввод!");
            }
        }
    }
}