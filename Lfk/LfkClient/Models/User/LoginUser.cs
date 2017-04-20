using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.Models.User
{
    /// <summary>
    /// Модель экземпляра пользователя при входе в систему
    /// </summary>
    public class LoginUser : AbstractUser
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}