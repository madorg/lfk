using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using LfkGUI.Base;
namespace LfkGUI.Utility
{
    public static class CustomCommands
    {
        public static readonly RoutedCommand RemoveTreeViewItem = new RoutedCommand(
            "Remove", typeof(RemovableTreeViewItem), new InputGestureCollection()
            {
                new KeyGesture(Key.Delete,ModifierKeys.Control)
            });
    }
}
