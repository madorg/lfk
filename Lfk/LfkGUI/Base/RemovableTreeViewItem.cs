using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using LfkGUI.Utility;
namespace LfkGUI.Base
{
    class RemovableTreeViewItem : TreeViewItem
    {
        public RemovableTreeViewItem(): base()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem removeMenuItem = new MenuItem();
            removeMenuItem.Icon = new PackIcon() { Kind = PackIconKind.CloseBoxOutline};
            removeMenuItem.Header = "Remove";
            this.SetResourceReference(BaseWindow.StyleProperty, "MaterialDesignTreeViewItem");
            removeMenuItem.Command = CustomCommands.RemoveTreeViewItem;
            contextMenu.Items.Add(removeMenuItem);
            this.PreviewMouseRightButtonDown += RemovableTreeViewItem_PreviewMouseRightButtonDown;
            this.ContextMenu = contextMenu;
        }

        private void RemovableTreeViewItem_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.IsSelected = true;
        }
    }
}
