using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using LfkClient.Models.Repository;
using LfkClient.Models.User;
using MahApps.Metro.Controls.Dialogs;
using LfkGUI.Repository;
using MahApps.Metro.Controls;

namespace LfkGUI.Base
{
    public class BaseWindow : MetroWindow
    {
        private Flyout settingsFlyout;
        public BaseWindow()
        {

            this.ApplyStyle();
        }

        private void ApplyStyle()
        {
            #region Настройка стиля окна

            this.SetResourceReference(BaseWindow.BackgroundProperty, "MaterialDesignPaper");
            this.SetResourceReference(BaseWindow.FontFamilyProperty, "MaterialDesignFont");
            this.SetResourceReference(BaseWindow.BorderBrushProperty, "AccentColorBrush");

            this.BorderThickness = new Thickness(1);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            #endregion

            #region Добавление Fyout
            StackPanel stackPanel = new StackPanel() { Margin = new Thickness(10) };
            ToggleSwitch languageToggleSwitch = new ToggleSwitch()
            {
                Name = "LanguageToggleSwitch",
                Header = "Language",
                OnLabel = "Russian",
                OffLabel = "English"
            };
            languageToggleSwitch.IsCheckedChanged += LanguageToggleSwitch_IsCheckedChanged;
            stackPanel.Children.Add(languageToggleSwitch);
            ToggleSwitch themeToggleSwitch = new ToggleSwitch()
            {
                Name = "ThemeToggleSwitch",
                Header = "Color theme",
                OnLabel = "Light",
                OffLabel = "Dark",
            };
            themeToggleSwitch.IsCheckedChanged += ThemeToggleSwitch_IsCheckedChanged;
            stackPanel.Children.Add(themeToggleSwitch);
            settingsFlyout = new Flyout()
            {
                Name = "SettingsFlyout",
                Header = "Settings",
                Position = Position.Right,
                Background = App.Current.Resources["WindowTitleColorBrush"] as Brush,
                MinWidth = 200,
                Content = stackPanel
            };
            this.Flyouts = new FlyoutsControl();
            this.Flyouts.Items.Add(settingsFlyout);
            #endregion
            #region Добавление кнопки настроек на панели окна 
            Button settingsButton = new Button()
            {
                Name = "SettingsButton",
                Style = App.Current.Resources["MetroCircleButtonStyle"] as Style,
                Width = 32,
                Height = 32,
                BorderThickness = new Thickness(0),
                Content = new MaterialDesignThemes.Wpf.PackIcon()
                {
                    Kind = MaterialDesignThemes.Wpf.PackIconKind.Settings,
                    Width = 16,
                    Height = 16,
                    Foreground = App.Current.Resources["MaterialDesignBody"] as Brush
                }

            };
            settingsButton.Click += SettingsButton_Click;
            this.RightWindowCommands = new WindowCommands();
            this.RightWindowCommands.Items.Add(settingsButton);
            #endregion

            #region Настройка текста

            TextElement.SetFontSize(this, 13);
            TextElement.SetFontWeight(this, FontWeights.Regular);
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Ideal);
            TextOptions.SetTextRenderingMode(this, TextRenderingMode.Auto);

            #endregion
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            settingsFlyout.IsOpen = !settingsFlyout.IsOpen;
        }

        public void ThemeToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            if ((sender as ToggleSwitch).IsChecked.Value)
            {
                App.Current.Resources.MergedDictionaries.First(m => m.Source.OriginalString ==
                "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml").Source =
                new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml",
                UriKind.RelativeOrAbsolute);
            }
            else
            {
                App.Current.Resources.MergedDictionaries.First(m => m.Source.OriginalString ==
                "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml").Source =
                new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml",
                UriKind.RelativeOrAbsolute);
            }
        }

        private void LanguageToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
