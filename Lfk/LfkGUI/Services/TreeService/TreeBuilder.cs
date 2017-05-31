using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.Services.TreeService
{
    public class TreeBuilder
    {
        public TreeNode PseudoRoot;
        private string[][] filenames;
        public TreeBuilder()
        {
            PseudoRoot = new TreeNode() { Depth = -1 };
        }

        #region Построение дерева

        public TreeNode BuildTreeFromFilenames(string[] filenames)
        {
            PseudoRoot = new TreeNode() { Depth = -1 };
            this.filenames = ConvertFilenamesToPaths(filenames);
            BuildTree(0, 0);
            return PseudoRoot;
        }

        private TreeNode BuildTree(int i, int j)
        {
            TreeNode treeNode = null;

            if (i == filenames.GetLength(0))
            {
                return null;
            }
            else if (j == 0)
            {
                treeNode = BuildTreeNodeBranch(ref i, ref j);

                if (!PseudoRoot.ChildrenNodes.Any(n => n.Data == treeNode.Data))
                {
                    treeNode.ParentNode = PseudoRoot;
                    PseudoRoot.ChildrenNodes.Add(treeNode);
                }
            }
            else if (j == filenames[i].GetLength(0))
            {
                BuildTree(++i, 0);
            }
            else
            {
                treeNode = BuildTreeNodeBranch(ref i, ref j);
            }
            return treeNode;


        }

        private TreeNode BuildTreeNodeBranch(ref int i, ref int j)
        {
            TreeNode branch = new TreeNode() { Data = filenames[i][j], Depth = j };
            TreeNode node = BuildTree(i, ++j);
            if (node != null)
            {
                PseudoRoot.FindItem(ref branch, j);
                if (!branch.ChildrenNodes.Any(n => n.Data == node.Data))
                {
                    branch.ChildrenNodes.Add(node);
                    node.ParentNode = branch;
                }
            }
            return branch;
        }

        #endregion

        #region SHIT!!!
        private TreeNode FindNodeToRemove(TreeNode node, Func<TreeNode, bool> predicate)
        {
            if (predicate(node))
                return node;

            foreach (var item in node.ChildrenNodes.AsParallel())
            {
                var found = FindNodeToRemove(item, predicate);
                if (found != null)
                    return found;
            }
            return null;
        }
        public void AttachNodeToTree(TreeNode root, TreeNode node)
        {
            PseudoRoot = root;
            List<string> list = new List<string>();
            this.ConvertTreeToFilenames(node, ref list);
            filenames = ConvertFilenamesToPaths(list.ToArray());
            root = BuildTree(0, 0);

        }
        public void RemoveNodeFromTree(TreeNode source, TreeNode node)
        {
            TreeNode bufferNode = FindNodeToRemove(source, n => n.Data == node.Data && n.Depth == node.Depth);
            TreeNode parent = bufferNode.ParentNode;
            parent.ChildrenNodes.Remove(bufferNode);

            while (parent.ChildrenNodes.Count == 0)
            {
                parent.ParentNode.ChildrenNodes.Remove(parent);
                parent = parent.ParentNode;
            }
        }
        #endregion

        #region Получение списка файлов из элемента дерева

        private string ConvertTreeNodeToFullFileName(TreeNode treeNode)
        {
            string path = string.Empty;
            while (treeNode != null)
            {
                if (treeNode.Data != null)
                {
                    path = "\\" + treeNode.Data + path;
                }
                treeNode = treeNode.ParentNode;
            }

            return path;
        }

        public void ConvertTreeToFilenames(TreeNode treeNode, ref List<string> filenames)
        {
            if (treeNode.ChildrenNodes.Count == 0)
            {
                filenames.Add(ConvertTreeNodeToFullFileName(treeNode));
            }
            else
            {
                foreach (var item in treeNode.ChildrenNodes)
                {
                    ConvertTreeToFilenames(item, ref filenames);
                }
            }
        }
        #endregion


        private string[][] ConvertFilenamesToPaths(string[] filenames)
        {
            string[][] filepaths = new string[filenames.Length][];

            for (int i = 0; i < filepaths.Count(); i++)
            {
                filepaths[i] = filenames[i].Split('\\').Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();
            }
            return filepaths;
        }
    }
}
