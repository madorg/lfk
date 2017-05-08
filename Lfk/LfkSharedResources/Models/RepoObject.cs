using System;
using System.Collections;

namespace LfkSharedResources.Models
{
    public class RepoObject
    {
        public Guid Id { get; set; }
        //public string Hash { get; set; }
        public byte[] ByteHash { get; set; }
        public byte[] HuffmanTree { get; set; }
        public Guid IndexId { get; set; }
    }
}