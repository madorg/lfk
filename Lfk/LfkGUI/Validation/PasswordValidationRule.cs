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
    public class PasswordValidationRule : ValidationRule
    {
        private string regexPattern = @"^(?=.*\d)(?=.*[a-zA-Z]).{4,24}$";
        private Regex regex;

        public PasswordValidationRule()
        {
            regex = new Regex(regexPattern);
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string pass = (value ?? string.Empty).ToString();

            if (!regex.IsMatch(pass))
            {
                return new ValidationResult(false, "Пароль должен содержать минимум 1 цифру и одну заглавную букву а размер 4-24");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
