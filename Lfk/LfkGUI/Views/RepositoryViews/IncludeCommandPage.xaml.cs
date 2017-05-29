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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LfkGUI.Utility;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
namespace LfkGUI.Views.RepositoryViews
{
    /// <summary>
    /// Логика взаимодействия для IncludeCommandPage.xaml
    /// </summary>
    public partial class IncludeCommandPage : Page
    {
        Point startDragPoint;
        bool isDrag = false;
        public IncludeCommandPage()
        {
            InitializeComponent();
            SetFiles();
        }
        private async void SetFiles()
        {
            await TreeViewConverter.BuildFilesTreeViewItem(WorkingDirectoryFilesTreeView,
                LfkClient.Repository.Repository.GetInstance().GetUnincludedFiles());
            await TreeViewConverter.BuildFilesTreeViewItem(IncludedFilesTreeView,
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
            if (treeView != null && treeView.Items.Count > 0)
            {

                object temp = treeView.SelectedItem;
                DataObject data = null;

                if (temp != null)
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
            if (item != null)
            {
                List<string> files = TreeViewConverter.ParseTreeViewItemToFullFilenames(item);
                TreeViewConverter.BuildFilesTreeViewItem(IncludedFilesTreeView, files.ToArray());
                LfkClient.Repository.Repository.GetInstance().Include(files);
                TreeViewConverter.RemoveTreeViewItem(item as TreeViewItem);
            }
        }

        private async void WorkingDirectoryRemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MetroWindow mw = App.Current.MainWindow as MetroWindow;
            MessageDialogResult result =
                await mw.ShowMessageAsync("You can't do this", "delete files from explore",
                               MessageDialogStyle.Affirmative);
        }

        private async void IncludedRemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ItemsControl item = e.OriginalSource as ItemsControl;
            if (item.Parent != null)
            {
                TreeViewItem treeViewItem = item as TreeViewItem;
                List<string> unincludedFiles = TreeViewConverter.ParseTreeViewItemToFullFilenames(treeViewItem);
                LfkClient.Repository.Repository.GetInstance().Uninclude(unincludedFiles);
                TreeViewConverter.RemoveTreeViewItem(treeViewItem);
                await TreeViewConverter.BuildFilesTreeViewItem(
                    WorkingDirectoryFilesTreeView,
                    unincludedFiles.ToArray());
              
            }

        }
    }
}
