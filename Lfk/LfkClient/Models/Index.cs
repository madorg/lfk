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
            FileName = new List<string>();
            Hash = new List<string>();
        }

        public Guid Id { get; set; }
        public List<string> FileName { get; set; }
        public List<string> Hash { get; set; }
    }
}