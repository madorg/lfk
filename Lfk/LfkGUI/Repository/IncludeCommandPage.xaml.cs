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
            BuildTreeView(tree, temp, 0, 0);
        }

        private void FindNode(ItemsControl node, string[] filepaths, int branchNumber, int branchDepth,ref TreeViewItem item)
        {
            if(branchNumber < node.Items.Count &&
                branchDepth < filepaths.Length)
            {
                if ((node.Items[branchNumber] as TreeViewItem).Header.ToString() == filepaths[branchDepth])
                {
                    if (branchDepth == filepaths.Length-1)
                    {
                        item = node.Items[branchNumber] as TreeViewItem;
                    }
                    FindNode(node.Items[branchNumber] as ItemsControl, filepaths, 0, ++branchDepth, ref item);
                }
                else if (node.Items.Count != 0)
                {
                    FindNode(node, filepaths, ++branchNumber, branchDepth,ref item);
                }
            }
           
        }
        private TreeViewItem BuildTreeView(TreeView root, string[][] filenames, int i, int j)
        {
            TreeViewItem branch = null;
            if (i == filenames.GetLength(0))
            {
                return branch;
            }
            else if (j == 0)
            {
                branch = new TreeViewItem() { Header = filenames[i][j] };
                TreeViewItem node = BuildTreeView(root, filenames, i, ++j);
                if (node != null)
                {
                    string[] pathToFind = new string[j];
                    Array.Copy(filenames[i], pathToFind, j);

                    TreeViewItem item = null;
                    FindNode(root, pathToFind, 0, 0, ref item);
                    if (item != null)
                    {
                        if (!item.Items.Contains(node))
                        {
                            item.Items.Add(node);
                        }
                        branch = item;
                    }
                    else
                    {
                        branch.Items.Add(node);
                    }
                }
                if (!root.Items.Contains(branch))
                {
                    root.Items.Add(branch);
                }
            }
            else if (j == filenames[i].GetLength(0))
            {
                BuildTreeView(root, filenames, ++i, 0);
            }
            else
            {
                branch = new TreeViewItem() { Header = filenames[i][j] };
                TreeViewItem node = BuildTreeView(root, filenames, i, ++j);
                if (node != null)
                {
                    string[] pathToFind = new string[j];
                    Array.Copy(filenames[i], pathToFind, j);
                    TreeViewItem item = null;
                    FindNode(root, pathToFind, 0, 0, ref item);
                    if (item != null)
                    {
                        if (!item.Items.Contains(node))
                        {
                            item.Items.Add(node);
                        }
                        branch = item;
                    }
                    else
                    {
                        branch.Items.Add(node);
                    }
                }
            }
            return branch;
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
