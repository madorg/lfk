using LfkClient.Authorization;
using LfkGUI.Services;
using LfkSharedResources.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using LfkGUI.Views.RepositoryManagementViews;
using LfkGUI.ViewModels.RepositoryManagementViewModels;
using System.Collections.ObjectModel;
using LfkGUI.Validation;

namespace LfkGUI.ViewModels.AuthorizationViewModels
{
    public class LoginViewModel : BasicViewModel
    {
        private DialogService dialogService;
        private WindowsService windowsService;

        public LoginViewModel(LoginUser loginUser, DialogService dService, WindowsService windowService)
        {
            ValidationErrors = new ObservableCollection<ValidationError>();
            LoginUser = loginUser;
            dialogService = dService;
            windowsService = windowService;
        }

        #region Свойства
        public ObservableCollection<ValidationError> ValidationErrors { get; private set; }
        public LoginUser LoginUser { get; set; }

        public string Email
        {
            get
            {
                return LoginUser.Email;
            }
            set
            {
                LoginUser.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get
            {
                return LoginUser.Password;
            }
            set
            {
                LoginUser.Password = value;
                OnPropertyChanged("Password");
            }
        }

        #endregion

        #region Команды

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand(Login));
            }
        }

        #endregion

        #region Методы обработки комманд

        private void Login(object obj)
        {
            Guid userId = Guid.Empty;
            string message = string.Empty;

            if (Authorizator.TryLogin(LoginUser, out message, out userId))
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
