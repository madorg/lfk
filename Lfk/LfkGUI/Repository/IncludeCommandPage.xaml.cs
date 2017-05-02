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
            BuildFilesTreeViewItem(WorkingDirectoryFilesTreeView);
        }

        private void BuildFilesTreeViewItem(TreeView tree/*string[] filenames*/)
        {
            string[][] temp = new string[5][]
            {
               new string[] {"folder1", "file1.txt" },
               new string[] {"folder1","folder3", "file2.txt" },
               new string[] {"folder2", "file3.txt" },
               new string[] {"file4.txt" },
               new string[] {"folder1","folder3", "file5.txt" }
            };
            BuildTreeViewItemNode(tree, temp, 0, 0);
        }

        private TreeViewItem FindNode(ItemsControl root, string[] filepath, int depth, int pathdepth)
        {
            TreeViewItem node = null;
            for (int i = 0; i < root.Items.Count; i++)
            {
                if ((root.Items[i] as TreeViewItem).Header.ToString() == filepath[pathdepth])
                {
                    FindNode(root.Items[i] as ItemsControl, filepath, ++depth, pathdepth);
                }
            }
            return node;
        }

        private bool CheckIfNodeExists(string[][] filenames, int row, int column)
        {
            bool rc = false;
            for (int i = 0; i < filenames.GetLength(0); i++)
            {
                if (column < filenames[i].GetLength(0) && filenames[i][column] == filenames[row][column])
                {
                    if (i != row)
                    {
                        rc = true;
                        break;
                    }
                }
            }
            return rc;
        }
        private TreeViewItem BuildTreeViewItemNode(TreeView root, string[][] filenames, int i, int j)
        {
            TreeViewItem returnValue = null;
            if (i == filenames.GetLength(0))
            {
                return returnValue;
            }
            else if (j == 0)
            {
                returnValue = new TreeViewItem() { Header = filenames[i][j] };
                TreeViewItem node = BuildTreeViewItemNode(root, filenames, i, ++j);
                if (node != null)
                {
                    if (CheckIfNodeExists(filenames, i, j) && (FindNode(root, filenames[i], 0, j - 1) != null))
                    {
                        returnValue = FindNode(root, filenames[i], 0, j - 1);
                    }
                    else
                    {
                        returnValue.Items.Add(node);
                    }

                }
                root.Items.Add(returnValue);
            }
            else if (j == filenames[i].GetLength(0))
            {
                BuildTreeViewItemNode(root, filenames, ++i, 0);
            }
            else
            {
                returnValue = new TreeViewItem() { Header = filenames[i][j] };
                TreeViewItem node = BuildTreeViewItemNode(root, filenames, i, ++j);
                if (node != null)
                {
                    if (CheckIfNodeExists(filenames, i, j) && (FindNode(root, filenames[i], 0, j - 1) != null))
                    {
                        returnValue = FindNode(root, filenames[i], 0,j-1);
                    }
                    else
                    {
                        returnValue.Items.Add(node);
                    }
                }
            }
            return returnValue;
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
            LfkClient.Repository.Repository.GetInstance().Include(new List<string> {
                (e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem).Header.ToString()
            });
        }
    }
}
