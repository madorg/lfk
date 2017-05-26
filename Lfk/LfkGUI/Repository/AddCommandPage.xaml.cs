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
            SetChangedFiles();
        }

        private async void SetChangedFiles()
        {
            TreeViewConverter.BuildFilesTreeViewItem(ChangedFilesTreeView,
                await LfkClient.Repository.Repository.GetInstance().GetChangedFiles());
            TreeViewConverter.BuildFilesTreeViewItem(AddedFilesTreeView,
               await LfkClient.Repository.Repository.GetInstance().GetChangedFilesAfterParentCommit());
        }

        private void ChangedFilesTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startDragPoint = e.GetPosition(null);
        }

        private void StartDrag(object sender, MouseEventArgs e)
        {
            try
            {
                TreeView treeView = sender as TreeView;
                isDrag = true;
                if (treeView != null && treeView.Items.Count > 0)
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
            catch (Exception)
            {
               
            }
           
        }

        private void ChangedFilesTreeView_MouseMove(object sender, MouseEventArgs e)
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
            if (item != null)
            {
                List<string> files = TreeViewConverter.ParseTreeViewItemToFullFilenames(item);
                TreeViewConverter.BuildFilesTreeViewItem(AddedFilesTreeView, files.ToArray());

                LfkClient.Repository.Repository.GetInstance()
                    .Add(files);

                ((item as ItemsControl).Parent as ItemsControl).Items.Remove(item);
            }
        }

        private void ChangedFilesRemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Base.RemovableTreeViewItem item = e.OriginalSource as Base.RemovableTreeViewItem;
            if(item!= null)
            {
                item.ContextMenu.IsOpen = false;
            }
        }

        private async void AddedFilesRemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ItemsControl item = e.OriginalSource as ItemsControl;
            if (item.Parent != null)
            {
                LfkClient.Repository.Repository.GetInstance().Reset(
                    TreeViewConverter.ParseTreeViewItemToFullFilenames(item as TreeViewItem));

                ChangedFilesTreeView.Items.Clear();
                TreeViewConverter.BuildFilesTreeViewItem(ChangedFilesTreeView,
                    await LfkClient.Repository.Repository.GetInstance().GetChangedFiles());
                ItemsControl parent = item.Parent as ItemsControl;
                if (parent != null)
                {
                    parent.Items.Remove(item);
                }
            }

        }
    }
}
