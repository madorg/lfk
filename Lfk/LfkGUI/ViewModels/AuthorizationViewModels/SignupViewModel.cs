using LfkClient.Authorization;
using LfkGUI.Services;
using LfkGUI.Validation;
using LfkGUI.Views.RepositoryManagementViews;
using LfkSharedResources.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ValidationErrors = new ObservableCollection<ValidationError>();
            SignupUser = signupUser;
            windowsService = windowService;
            this.dialogService = dialogService;
        }
        #region Свойства
        public ObservableCollection<ValidationError> ValidationErrors { get; private set; }

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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
        #endregion

        #region Команды

        private RelayCommand signupCommand;
        public RelayCommand SignupCommand
        {
            get
            {
                return signupCommand ?? (signupCommand = new RelayCommand(Signup,obj=> {
                    return true;
                }));
            }
        }

        #endregion

        #region Методы обработки комманд

        private void Signup(object obj)
        {
            Guid userId = Guid.Empty;
            string message = string.Empty;
            if (Password != ConfirmPassword)
            {
                dialogService.ShowMessage(windowsService.CurrentOpenedWindow, "Пароли не совпадают");
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
                dialogService.ShowMessage(windowsService.CurrentOpenedWindow, message);
            }
        }

        #endregion

    }
}
