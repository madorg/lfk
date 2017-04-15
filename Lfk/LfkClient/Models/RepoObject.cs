using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.Models
{
    public class RepoObject
    {
        public int Hash { get; set; }
        public int FileId { get; set; }
        public int IndexId { get; set; }
    }
}
