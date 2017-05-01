using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
namespace LfkGUI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var flyout = App.Current.Resources["SettingsFlyout"] as Flyout;
            flyout.IsOpen = !flyout.IsOpen;

        }
    }
}
