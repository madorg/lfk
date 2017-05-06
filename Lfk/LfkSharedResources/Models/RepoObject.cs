using System;

namespace LfkSharedResources.Models
{
    public class RepoObject
    {
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public Guid IndexId { get; set; }
    }
}