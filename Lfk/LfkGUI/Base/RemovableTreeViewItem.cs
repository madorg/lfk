using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
namespace LfkGUI.Base
{
    class RemovableTreeViewItem : TreeViewItem
    {
        public RemovableTreeViewItem(): base()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem removeMenuItem = new MenuItem();
            removeMenuItem.Icon = new PackIcon() { Kind = PackIconKind.CloseBoxOutline};
            removeMenuItem.Header = "remove";
            this.SetResourceReference(BaseWindow.StyleProperty, "MaterialDesignTreeViewItem");
            removeMenuItem.Click += RemoveMenuItem_Click;
            contextMenu.Items.Add(removeMenuItem);
            this.ContextMenu = contextMenu;
        }

        private void RemoveMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ItemsControl parent = this.Parent as ItemsControl;
            if(parent!= null)
            {
                parent.Items.Remove(this);
            }
        }
    }
}
