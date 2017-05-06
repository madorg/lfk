using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkSharedResources.Models.User
{
    /// <summary>
    /// Модель экземпляра пользователя при входе в систему
    /// </summary>
    public class LoginUser : AbstractUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}