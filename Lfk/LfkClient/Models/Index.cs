using System;
using System.Collections.Generic;

namespace LfkClient.Models
{
    public class Index
    {
        public Index()
        {
            RepoObjectIdAndFileName = new Dictionary<Guid, string>();
        }

        public Guid Id { get; set; }
        public Dictionary<Guid, string> RepoObjectIdAndFileName { get; set; }
        public List<Guid> BlobsId { get; set; }
    }
}