﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using LfkGUI.Base;
namespace LfkGUI.Utility
{
    public class TreeViewConverter
    {
        public static TreeViewItem BuildTreeView(ItemsControl root, string[][] filenames, int i, int j)
        {
            RemovableTreeViewItem branch = null;
            if (i == filenames.GetLength(0))
            {
                return branch;
            }
            else if (j == 0)
            {
                branch = BuildBranch(root, filenames, ref i, ref j);
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
                branch = BuildBranch(root, filenames, ref i, ref j);
            }
            return branch;
        }

        private static RemovableTreeViewItem BuildBranch(ItemsControl root, string[][] filenames, ref int i, ref int j)
        {
            RemovableTreeViewItem branch = new RemovableTreeViewItem() { Header = filenames[i][j] };
            RemovableTreeViewItem node = BuildTreeView(root, filenames, i, ++j) as RemovableTreeViewItem;
            if (node != null)
            {
                string[] pathToFind = new string[j];
                Array.Copy(filenames[i], pathToFind, j);
                FindNode(root, pathToFind, 0, 0, ref branch);
                if(!branch.Items.Contains(node))
                branch.Items.Add(node);
            }
            return branch;
        }

        private static void FindNode(ItemsControl node, string[] filepaths, int branchNumber, int branchDepth, ref RemovableTreeViewItem item)
        {
            if (branchNumber < node.Items.Count &&
                branchDepth < filepaths.Length)
            {
                if ((node.Items[branchNumber] as RemovableTreeViewItem).Header.ToString() == filepaths[branchDepth])
                {
                    if (branchDepth == filepaths.Length - 1)
                    {
                        item = node.Items[branchNumber] as RemovableTreeViewItem;
                    }
                    FindNode(node.Items[branchNumber] as ItemsControl, filepaths, 0, ++branchDepth, ref item);
                }
                else if (node.Items.Count != 0)
                {
                    FindNode(node, filepaths, ++branchNumber, branchDepth, ref item);
                }
            }
        }

        private static void GetParentPath(RemovableTreeViewItem node,ref StringBuilder path)
        {
            if (node.Parent is RemovableTreeViewItem)
            {
                GetParentPath(node.Parent as RemovableTreeViewItem,ref path);
            }
            path.Append(string.Concat("\\" + node.Header.ToString()));
        }

        public static List<string> ParseTreeViewItemToFullFilenames(TreeViewItem node)
        {
            List<string> list = new List<string>();
            StringBuilder prefix = new StringBuilder();
            RemovableTreeViewItem parent = node.Parent as RemovableTreeViewItem;
            if(parent != null) {
                GetParentPath(parent,ref prefix);
            }
            prefix.Append("\\" + node.Header.ToString());
            ParseTreeViewItem(node as RemovableTreeViewItem, ref list, prefix.ToString());
            return list;
        }
        private static void ParseTreeViewItem(RemovableTreeViewItem root, ref List<string> files, string prefix)
        {
            foreach (var item in root.Items)
            {
                if (Path.HasExtension((item as RemovableTreeViewItem).Header.ToString()))
                {
                    files.Add(prefix + "\\" + (item as RemovableTreeViewItem).Header.ToString());
                }
                ParseTreeViewItem(item as RemovableTreeViewItem, ref files, prefix + "\\" + (item as RemovableTreeViewItem).Header.ToString());
            }
            if (!root.HasItems && files.Count == 0)
            {
                files.Add("\\" + root.Header.ToString());
            }
        }
    }
}

