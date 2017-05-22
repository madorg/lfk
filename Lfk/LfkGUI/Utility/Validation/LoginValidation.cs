using LfkSharedResources.Models.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LfkGUI.Utility.Validation
{
    public class LoginValidation
    {
        private static Regex regularExpression;

        static LoginValidation()
        {
            regularExpression = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }

        public static bool IsValid(LoginUser loginUser)
        {
            return regularExpression.IsMatch(loginUser.Email) && !string.IsNullOrWhiteSpace(loginUser.Password);
        }
    }
}
