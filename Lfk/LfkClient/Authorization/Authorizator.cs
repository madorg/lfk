using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkClient.Models.User;
using LfkClient.Serialization.Json;
using LfkClient.ServerConnection;

namespace LfkClient.Authorization
{
    /// <summary>
    /// Фасад авторизации пользователя, перенаправляющий запросы пользователя в соответствующие обработчики
    /// </summary>
    public class Authorizator
    {
        public static bool TryLogin(AbstractUser abstractUser)
        {
            string data = JsonSerializer.SerializeObject(abstractUser);

            return true;
        }

        public static bool TrySignup(AbstractUser abstractUser)
        {
            string data = JsonSerializer.SerializeObject(abstractUser);

            return true;
        }
    }
}