using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LfkGUI.Utility.Validation
{
    public class EmailValidation
    {
        private static Regex regularExpression;

        static EmailValidation()
        {
            regularExpression = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }
        public static bool IsValid(string email)
        {
            return regularExpression.IsMatch(email);
        }
    }
}
