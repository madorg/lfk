using LfkClient.Authorization;
using LfkGUI.Services;
using LfkGUI.Views.RepositoryManagementViews;
using LfkSharedResources.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LfkGUI.ViewModels.AuthorizationViewModels
{
    public class SignupViewModel : BasicViewModel
    {
        private DialogService dialogService;
        private WindowsService windowsService;

        public SignupUser SignupUser { get; set; }

        public SignupViewModel(SignupUser signupUser, DialogService dialogService,WindowsService windowService)
        {
            SignupUser = signupUser;
            windowsService = windowService;
            this.dialogService = dialogService;
        }
        #region Свойства

        public string Name
        {
            get
            {
                return SignupUser.Name;
            }
            set
            {
                SignupUser.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Email
        {
            get
            {
                return SignupUser.Email;
            }
            set
            {
                SignupUser.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get
            {
                return SignupUser.Password;
            }
            set
            {
                SignupUser.Password = value;
                OnPropertyChanged("Password");
            }
        }

        #endregion

        #region Команды

        private RelayCommand signupCommand;
        public RelayCommand SignupCommand
        {
            get
            {
                return signupCommand ?? (signupCommand = new RelayCommand(Signup));
            }
        }

        #endregion

        #region Методы обработки комманд

        private void Signup(object obj)
        {
            Guid userId = Guid.Empty;
            string message = string.Empty;
            var objects = obj as List<object>;
            var passwords = objects.Cast<PasswordBox>().ToList();
            if (passwords[0].Password != passwords[1].Password)
            {
                dialogService.ShowMessage(App.Current.MainWindow, "Пароли не совпадают");
                SignupUser.Password = passwords[0].Password;
            }
            else if (Authorizator.TrySignup(SignupUser, out message, out userId))
            {
                App.User = new User()
                {
                    Id = userId
                };
                windowsService.NavigateToWindow(new RepositoryManagementWindow());
            }
            else
            {
                dialogService.ShowMessage(App.Current.MainWindow, message);
            }
        }

        #endregion

    }
}
