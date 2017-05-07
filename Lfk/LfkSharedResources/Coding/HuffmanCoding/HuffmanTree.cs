using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkSharedResources.Coding.HuffmanCoding
{
    public class HuffmanTree
    {
        public List<HuffmanTreeNode> Nodes { get; private set; }

        /// <summary>
        /// Информация об узле дерева Хаффмана, хранит символ, число вхождений и кодировку
        /// </summary>
        public class HuffmanTreeNodeInfo
        {
            public char Symbol { get; set; }
            public int Occurrences { get; set; }
            public List<bool> Code { get; set; }

            public HuffmanTreeNodeInfo()
            {
                Code = new List<bool>();
            }
        }

        /// <summary>
        /// Узел дерева Хаффмана, хранит данные и информацию о потомках
        /// </summary>
        public class HuffmanTreeNode
        {
            public HuffmanTreeNodeInfo Data { get; set; }
            public HuffmanTreeNode Left { set; get; }
            public HuffmanTreeNode Right { set; get; }
        }

        /// <summary>
        /// Строит дерево Хаффмана на основе указанной строки
        /// </summary>
        /// <param name="input">Входная строка, на основе которой будет строится дерево Хаффмана</param>
        public void BuildHuffmanTree(string input)
        {
            Nodes = new List<HuffmanTreeNode>();

            foreach (char symbol in input)
            {
                if (Nodes.Any(item => item.Data.Symbol == symbol))
                {
                    Nodes.First(item => item.Data.Symbol == symbol).Data.Occurrences++;
                }
                else
                {
                    Nodes.Add(new HuffmanTreeNode()
                    {
                        Data = new HuffmanTreeNodeInfo()
                        {
                            Symbol = symbol,
                            Occurrences = 1
                        }
                    });
                }
            }

            while (Nodes.Count > 1)
            {
                Nodes = Nodes.OrderBy(item => item.Data.Occurrences).ToList();

                HuffmanTreeNode tmpNodeLeft = Nodes[0];
                HuffmanTreeNode tmpNodeRight = Nodes[1];

                Nodes.RemoveRange(0, 2);

                HuffmanTreeNode newNode = new HuffmanTreeNode()
                {
                    Left = tmpNodeLeft,
                    Right = tmpNodeRight,
                    Data = new HuffmanTreeNodeInfo()
                    {
                        Symbol = '\0',
                        Occurrences = tmpNodeLeft.Data.Occurrences + tmpNodeRight.Data.Occurrences
                    }
                };

                Nodes.Insert(0, newNode);
            }
        }

        /// <summary>
        /// Осуществляет проход по дереву Хаффмана с целью задать каждому листу свой код
        /// </summary>
        /// <param name="node">Узел, относительно которого строится кодировка</param>
        private void Traverse(HuffmanTreeNode node)
        {
            if (node != null)
            {
                if (node.Left != null)
                {
                    node.Left.Data.Code.AddRange(node.Data.Code);
                    node.Left.Data.Code.Add(false);
                    Traverse(node.Left);
                }

                if (node.Right != null)
                {
                    node.Right.Data.Code.AddRange(node.Data.Code);
                    node.Right.Data.Code.Add(true);
                    Traverse(node.Right);
                }
            }
        }

        /// <summary>
        /// Осуществляет поиск узла в дереве Хаффмана, содержащего указанный символ
        /// </summary>
        /// <param name="node">Узел, относительно которого осуществляется поиск</param>
        /// <param name="symbol">Искомый символ</param>
        private HuffmanTreeNode Find(HuffmanTreeNode node, char symbol)
        {
            if (node != null)
            {
                if (node.Data.Symbol == symbol)
                {
                    return node;
                }

                HuffmanTreeNode foundedNode;

                if ((foundedNode = Find(node.Left, symbol)) != null)
                {
                    return foundedNode;
                }

                if ((foundedNode = Find(node.Right, symbol)) != null)
                {
                    return foundedNode;
                }
            }

            return null;
        }


    }
}