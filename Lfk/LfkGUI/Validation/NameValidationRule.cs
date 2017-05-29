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
    public class NameValidationRule : ValidationRule
    {
        private string regexPattern = @"([A-Z0-9a-z]){3,24}";
        private Regex regex;

        public NameValidationRule()
        {
            regex = new Regex(regexPattern);
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value.ToString();

            if (!regex.IsMatch(email))
            {
                return new ValidationResult(false, "Недопустимое имя");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
