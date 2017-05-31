using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LfkGUI.Services.TreeService
{
    public class TreeNode
    {
        public string Data { get; set; }
        public int Depth { get; set; }

        public TreeNode ParentNode { get; set; }
        public ObservableCollection<TreeNode> ChildrenNodes { get; set; }

        public TreeNode()
        {
            ChildrenNodes = new ObservableCollection<TreeNode>();
        }

        public bool FindItem(ref TreeNode treeNode, int depth)
        {
            for (int i = 0; i < ChildrenNodes.Count; i++)
            {
                if (ChildrenNodes[i].Data == treeNode.Data &&
                    ChildrenNodes[i].Depth == treeNode.Depth)
                {
                    if (ChildrenNodes[i].Depth == depth - 1)
                    {
                        treeNode = ChildrenNodes[i];
                        return true;
                    }
                    ChildrenNodes[i].FindItem(ref treeNode, depth);
                }
                else
                {
                    ChildrenNodes[i].FindItem(ref treeNode, depth);
                }
            }
            return false;
        }
    }
}
