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
using LfkGUI.Utility;
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

            TreeViewConverter.BuildFilesTreeViewItem(IncludedFilesTreeView,
                LfkClient.Repository.Repository.GetInstance().GetIncludedFiles());
        }

        private void FilesTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startDragPoint = e.GetPosition(null);
        }

        private void StartDrag(object sender, MouseEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            isDrag = true;
            if (treeView.Items.Count > 0)
            {

                object temp = treeView.SelectedItem;
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
            TreeViewItem item = e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem;
            List<string> files = TreeViewConverter.ParseTreeViewItemToFullFilenames(item);
            TreeViewConverter.BuildFilesTreeViewItem(AddedFilesTreeView, files.ToArray());
            LfkClient.Repository.Repository.GetInstance()
                .Add(files);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ItemsControl item = e.Source as ItemsControl;
            if (item.Parent != null)
            {
                ItemsControl parent = item.Parent as ItemsControl;
                if (parent != null)
                {
                    parent.Items.Remove(this);
                }
                //LfkClient.Repository.Repository.GetInstance().Uninclude(TreeViewConverter.ParseTreeViewItemToFullFilenames(item as TreeViewItem));
            }
        }
    }
}
