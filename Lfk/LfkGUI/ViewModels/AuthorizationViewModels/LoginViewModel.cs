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

namespace LfkGUI.ViewModels.AuthorizationViewModels
{
    public class LoginViewModel : BasicViewModel
    {
        private IDialogService dialogService;
        public LoginUser LoginUser { get; set; }
        private WindowsService windowsService;
        public LoginViewModel(LoginUser loginUser, IDialogService dService,WindowsService windowService)
        {
            LoginUser = loginUser;
            dialogService = dService;
            windowsService = windowService;
        }
        #region Свойства

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
            LoginUser.Password = (obj as PasswordBox).Password;

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
                dialogService.ShowMessage(App.Current.MainWindow, message);
            }
        }

        #endregion
    }
}
