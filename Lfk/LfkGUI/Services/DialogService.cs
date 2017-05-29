using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LfkGUI.Services
{
    public class DialogService : IDialogService
    {
        public async void ShowMessage(object window,string message)
        {
            MetroWindow metroWindow = window as MetroWindow;
            if(metroWindow!= null)
            {
                await metroWindow.ShowMessageAsync("Сообщение", message,MessageDialogStyle.Affirmative);
            } 
            
        }

        public async Task<bool> ShowMessageDialog(object window, string message)
        {
            bool rc = false;
            MetroWindow metroWindow = window as MetroWindow;
            if (metroWindow != null)
            {
                MessageDialogResult result =  await metroWindow.ShowMessageAsync("Сообщение", message, MessageDialogStyle.AffirmativeAndNegative);
                rc = (result == MessageDialogResult.Affirmative);
            }

            return rc;
        }
    }
}
