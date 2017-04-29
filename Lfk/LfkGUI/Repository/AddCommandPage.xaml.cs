using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LfkGUI.Repository
{
    /// <summary>
    /// Логика взаимодействия для AddCommandPage.xaml
    /// </summary>
    public partial class AddCommandPage : Page
    {
        Point startDragPoint;
        bool isDrag = false;

        public AddCommandPage()
        {
            InitializeComponent();
            foreach (string filename in LfkClient.Repository.Repository.GetInstance().GetWorkingDirectoryFiles())
            {
                IncludedFilesTreeView.Items.Add(new TreeViewItem() { Header = filename });
            }
        }
      
        private void FilesTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startDragPoint = e.GetPosition(null);
        }
        private void StartDrag(object sender, MouseEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            object itemParent = (treeView.SelectedItem as Control).Parent;
            isDrag = true;
            if ((treeView.SelectedItem as TreeViewItem).Header.ToString() != "Root")
            {
                object temp = treeView.SelectedItem;
                (itemParent as ItemsControl).Items.Remove(temp);
                DataObject data = null;
                data = new DataObject(typeof(TreeViewItem), temp);
                if (data != null)
                {
                    DragDropEffects effects = DragDropEffects.Move;
                    if (e.RightButton == MouseButtonState.Pressed)
                    {
                        effects = DragDropEffects.All;
                    }
                    DragDropEffects de = DragDrop.DoDragDrop(treeView, data, effects);
                }
                isDrag = false;
            }
        }

        private void FilesTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed ||
              e.RightButton == MouseButtonState.Pressed && !isDrag)
            {
                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - startDragPoint.X) >
                        SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - startDragPoint.Y) >
                        SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(sender, e);
                }
            }
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            (sender as ItemsControl).Items.Add(e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem);
            LfkClient.Repository.Repository.GetInstance().Add(new List<string> {
            (e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem).Header.ToString()
            });
        }
    }
}
