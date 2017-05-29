using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LfkGUI.Services
{
    public interface IDialogService
    {
        void ShowMessage(object window, string message);
        Task<bool> ShowMessageDialog(object window, string message);
    }
}
