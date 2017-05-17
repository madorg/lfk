using System;

namespace LfkSharedResources.Models
{
    public class Commit
    {
        public Guid Id { get; set; }
        public Index Index { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}