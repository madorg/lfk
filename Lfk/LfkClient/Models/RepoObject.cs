using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.Models
{
    public class RepoObject
    {
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public Guid   FileId { get; set; }
        public Guid   IndexId { get; set; }
    }
}
