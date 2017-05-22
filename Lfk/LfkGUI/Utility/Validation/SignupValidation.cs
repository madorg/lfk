using LfkSharedResources.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LfkGUI.Utility.Validation
{
    class SignupValidation
    {
        private static Regex regularExpression;

        static SignupValidation()
        {
            regularExpression = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }

        public static bool IsValid(SignupUser signupUser)
        {
            bool rc = true;
            if (!regularExpression.IsMatch(signupUser.Email))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(signupUser.Password))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(signupUser.Name))
            {
                return false;
            }
            return rc;
        }
    }
}
