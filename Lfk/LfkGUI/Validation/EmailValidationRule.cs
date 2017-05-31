using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LfkGUI.Validation
{
    public class EmailValidationRule : ValidationRule
    {
        private string regexPattern = @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
        private Regex regex;

        public EmailValidationRule()
        {
            regex = new Regex(regexPattern);
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value.ToString();

            if (!regex.IsMatch(email))
            {
                return new ValidationResult(false, "Недопустимое e-mail");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
