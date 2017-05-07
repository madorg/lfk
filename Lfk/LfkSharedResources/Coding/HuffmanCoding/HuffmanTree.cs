using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public HuffmanTree()
        {
            Nodes = new List<HuffmanTreeNode>();
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
        /// Осуществляет кодировку указанной строки на основе дереве Хаффмана
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public byte[] EncodeDataBasedOnHuffmanTree(string source)
        {
            List<bool> encoded = new List<bool>();

            Traverse(Nodes.First());

            foreach (char symbol in source)
            {
                encoded.AddRange(Find(Nodes.First(), symbol).Data.Code);
            }

            BitArray encodedBits = new BitArray(encoded.ToArray());

            byte[] encodedBytes = new byte[(encodedBits.Length / 8) + (encodedBits.Length % 8 == 0 ? 0 : 1)];
            encodedBits.CopyTo(encodedBytes, 0);

            return encodedBytes;
        }

        public string DecodeDataBasedOnHuffmanTree(byte[] encodedData)
        {
            HuffmanTreeNode currentNode = Nodes.First();
            BitArray encodedBits = new BitArray(encodedData);
            string decodedData = string.Empty;

            foreach (bool bit in encodedBits)
            {
                if (bit)
                {
                    if (currentNode.Right != null)
                    {
                        currentNode = currentNode.Right;
                    }
                }
                else
                {
                    if (currentNode.Left != null)
                    {
                        currentNode = currentNode.Left;
                    }
                }

                if (currentNode.Left == null && currentNode.Right == null)
                {
                    decodedData += currentNode.Data.Symbol;
                    currentNode = Nodes.First();
                }
            }

            return decodedData;
        }

        /// <summary>
        /// Осуществляет кодировку дерева Хаффмана в битовую последовательность
        /// </summary>
        /// <returns></returns>
        public byte[] EncodeHuffmanTree()
        {
            StringBuilder encodedTree = new StringBuilder();
            EncodeNode(Nodes.First(), ref encodedTree);
            return Encoding.UTF8.GetBytes(encodedTree.ToString());
        }

        /// <summary>
        /// Осуществляет декодировку дерева Хаффмана
        /// </summary>
        /// <param name="encodedHuffmanTree">Закодированное дерево Хаффмана</param>
        public void DecodeHuffmanTree(byte[] encodedHuffmanTree)
        {
            string encodedHuffmanTreeInString = Encoding.UTF8.GetString(encodedHuffmanTree);
            Nodes.Clear();
            Nodes.Add(DecodeNode(ref encodedHuffmanTreeInString));
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

        private void EncodeNode(HuffmanTreeNode node, ref StringBuilder encodedTree)
        {
            if (node.Left == null && node.Right == null)
            {
                encodedTree.Append('1');
                encodedTree.Append(node.Data.Symbol);
            }
            else
            {
                encodedTree.Append('0');
                EncodeNode(node.Left, ref encodedTree);
                EncodeNode(node.Right, ref encodedTree);
            }
        }

        private HuffmanTreeNode DecodeNode(ref string encodedHuffmanTree)
        {
            if (encodedHuffmanTree != string.Empty)
            {
                char symbol = encodedHuffmanTree.First();
                encodedHuffmanTree = encodedHuffmanTree.Remove(0, 1);

                if (symbol == '1')
                {
                    symbol = encodedHuffmanTree.First();
                    encodedHuffmanTree = encodedHuffmanTree.Remove(0, 1);
                    return new HuffmanTreeNode()
                    {
                        Left = null,
                        Right = null,
                        Data = new HuffmanTreeNodeInfo()
                        {
                            Symbol = symbol
                        }
                    };
                }
                else
                {
                    HuffmanTreeNode leftNode = DecodeNode(ref encodedHuffmanTree);
                    HuffmanTreeNode rightNode = DecodeNode(ref encodedHuffmanTree);

                    return new HuffmanTreeNode()
                    {
                        Left = leftNode,
                        Right = rightNode,
                        Data = new HuffmanTreeNodeInfo()
                        {
                            Symbol = '\0'
                        }
                    };
                }
            }

            return null;
        }
    }
}