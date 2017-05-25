using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LfkSharedResources.Coding.HuffmanCoding
{
    public class HuffmanTree
    {
        private List<HuffmanTreeNode> nodes;

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

        private HuffmanTree()
        {
            nodes = new List<HuffmanTreeNode>();
        }

        /// <summary>
        /// Создаёт и строит дерево Хаффмана на основе исходного текста
        /// </summary>
        /// <param name="source">Исходный текст, на основе которого строится дерево Хаффмана</param>
        public HuffmanTree(string source) : this()
        {
            BuildHuffmanTree(source);
        }

        /// <summary>
        /// Создаёт и строит дерево Хаффмана на основе закодированного дерева Хаффмана
        /// </summary>
        /// <param name="source">Закодированное дерево Хаффмана</param>
        public HuffmanTree(byte[] source) : this()
        {
            DecodeHuffmanTree(source);
        }

        #region API

        /// <summary>
        /// Осуществляет кодировку указанной строки на основе дереве Хаффмана
        /// </summary>
        /// <param name="source">Строка, для которой будет осуществляться кодировка</param>
        /// <returns></returns>
        public byte[] EncodeData(string source)
        {
            byte[] encodedBytes = null;

            if (source != string.Empty)
            {
                List<bool> encoded = new List<bool>();

                Traverse(nodes.FirstOrDefault());  // ok

                foreach (char symbol in source)
                {
                    encoded.AddRange(Find(nodes.First(), symbol).Data.Code);
                }  // ok

                BitArray encodedBits = new BitArray(encoded.ToArray());  // ok

                byte[] size = new byte[4];                                            // ???
                size = BitConverter.GetBytes(Encoding.Unicode.GetByteCount(source));  // ???

                encodedBytes = new byte[((encodedBits.Length - 1) / 8 + 1) + 4];      // ???????
                for (int i = 0; i < 4; i++)
                {
                    encodedBytes[i] = size[i];
                }

                encodedBits.CopyTo(encodedBytes, 4);

                BitArray ba = new BitArray(encodedBytes);
            }
            else
            {
                encodedBytes = new byte[4];
                byte[] size = BitConverter.GetBytes(Encoding.Unicode.GetByteCount(source));
                for (int i = 0; i < 4; i++)
                {
                    encodedBytes[i] = size[i];
                }
            }

            return encodedBytes;
        }

        /// <summary>
        /// Осуществляет декодировку указанных данных на основе дерева Хаффмана
        /// </summary>
        /// <param name="encodedData">Данные для декодировки</param>
        public string DecodeData(byte[] encodedData)
        {
            HuffmanTreeNode currentNode = nodes.First();

            byte[] sizeInBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                sizeInBytes[i] = encodedData[i];
            }
            int sizeInBits = BitConverter.ToInt32(sizeInBytes, 0);
            int size = sizeInBits / 2;  // на каждый символ по два байта

            BitArray encodedBits = new BitArray(encodedData);
            string decodedData = string.Empty;

            for (int i = 32; i < encodedBits.Length; i++)
            {
                if (encodedBits[i])
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
                    currentNode = nodes.First();
                }
            }

            decodedData = decodedData.Substring(0, size);
            return decodedData;
        }

        /// <summary>
        /// Осуществляет кодировку дерева Хаффмана в битовую последовательность
        /// </summary>
        public byte[] EncodeHuffmanTree()
        {
            StringBuilder encodedTree = new StringBuilder();
            EncodeNode(nodes.FirstOrDefault(), ref encodedTree);
            return Encoding.UTF8.GetBytes(encodedTree.ToString());
        }        

        #endregion

        #region Служебные (приватные) методы

        /// <summary>
        /// Строит дерево Хаффмана на основе указанной строки
        /// </summary>
        /// <param name="input">Входная строка, на основе которой будет строится дерево Хаффмана</param>
        private void BuildHuffmanTree(string input)
        {
            foreach (char symbol in input)
            {
                if (nodes.Any(item => item.Data.Symbol == symbol))
                {
                    nodes.First(item => item.Data.Symbol == symbol).Data.Occurrences++;
                }
                else
                {
                    nodes.Add(new HuffmanTreeNode()
                    {
                        Data = new HuffmanTreeNodeInfo()
                        {
                            Symbol = symbol,
                            Occurrences = 1
                        }
                    });
                }
            }

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(item => item.Data.Occurrences).ToList();

                HuffmanTreeNode tmpNodeLeft = nodes[0];
                HuffmanTreeNode tmpNodeRight = nodes[1];

                nodes.RemoveRange(0, 2);

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

                nodes.Insert(0, newNode);
            }
        }

        /// <summary>
        /// Осуществляет декодировку дерева Хаффмана
        /// </summary>
        /// <param name="encodedHuffmanTree">Закодированное дерево Хаффмана</param>
        public void DecodeHuffmanTree(byte[] encodedHuffmanTree)
        {
            string encodedHuffmanTreeInString = Encoding.UTF8.GetString(encodedHuffmanTree);
            nodes.Clear();
            nodes.Add(DecodeNode(ref encodedHuffmanTreeInString));
        }

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
            if (node != null)
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

        #endregion
    }
}