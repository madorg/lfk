using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkClient.Models.User;

namespace LfkClient.Authorization
{
    /// <summary>
    /// Фасад авторизации пользователя, перенаправляющий запросы пользователя в соответствующие обработчики
    /// </summary>
    public class Authorizator
    {
        public static bool TryLogin(AbstractUser abstractUser)
        {
            LoginUser loginUser = abstractUser as LoginUser;

            System.Windows.Forms.MessageBox.Show(loginUser.username + " - " + loginUser.password);

            return false;
        }

        public static bool TrySignup(AbstractUser abstractUser)
        {
            return false;
        }
    }
}