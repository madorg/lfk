using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;

namespace LfkGUI
{
    public partial class SharedResourcesDictionary : ResourceDictionary
    {
        public void SettingsButton_Click(object sender, EventArgs e)
        {
            var flyout = App.Current.Resources["SettingsFlyout"] as Flyout;
            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}
