using System;

namespace LfkSharedResources.Models
{
    public class RepoObject
    {
        public Guid Id { get; set; }
        public byte[] Hash { get; set; }
        public byte[] HuffmanTree { get; set; }
        public Guid FileId { get; set; }
        public Guid IndexId { get; set; }
    }
}