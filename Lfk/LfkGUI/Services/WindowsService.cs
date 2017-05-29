using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LfkGUI.Services
{
    public class WindowsService
    {
        public MetroWindow CurrentOpenedWindow { get; set; }

        public WindowsService(Window maintainedWindow)
        {
            CurrentOpenedWindow = maintainedWindow as MetroWindow;
        }

        public void NavigateToWindow(Window window)
        {
            window.Show();
            CurrentOpenedWindow.Close();
        }
    }
}
