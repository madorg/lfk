using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.Models
{
    public class Index
    {
        public Index()
        {
            RepoObjectId_FileName = new Dictionary<Guid, string>();
        }

        public Guid Id { get; set; }
        public Dictionary<Guid, string> RepoObjectId_FileName { get; set; }
    }
}