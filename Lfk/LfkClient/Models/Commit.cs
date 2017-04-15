using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.Models
{
    public class Commit
    {
        public int IndexId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public Commit Parent { get; set; }
    }
}
