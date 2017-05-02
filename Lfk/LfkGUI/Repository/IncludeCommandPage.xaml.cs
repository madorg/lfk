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
namespace LfkGUI.Repository
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
            BuildFilesTreeViewItem(WorkingDirectoryFilesTreeView,
                LfkClient.Repository.Repository.GetInstance().GetWorkingDirectoryFiles());
        }

        private void BuildFilesTreeViewItem(TreeView tree, string[] filespaths)
        {

            string[][] filenames = new string[filespaths.Length][];
            for (int i = 0; i < filespaths.Count(); i++)
            {
                filenames[i] = filespaths[i].Split('\\')
                    .Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();
            }
            TreeViewConverter.BuildTreeView(tree, filenames, 0, 0);
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
            BuildFilesTreeViewItem(IncludedFilesTreeView, files.ToArray());
            LfkClient.Repository.Repository.GetInstance()
                .Include(files);
        }

        private void IncludedFilesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }
    }
}
