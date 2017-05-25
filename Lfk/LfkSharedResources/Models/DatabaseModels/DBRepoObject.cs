using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LfkSharedResources.Models.DatabaseModels
{
    [Table(Name = "objects")]
    public class DBRepoObject
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "file_id")]
        public Guid FileId { get; set; }
        [Column(Name = "index_id")]
        public Guid IndexId { get; set; }
        [Column(Name = "data")]
        public byte[] Data { get; set; }
        [Column(Name = "tree")]
        public byte[] HuffmanTree { get; set; }       
    }
}