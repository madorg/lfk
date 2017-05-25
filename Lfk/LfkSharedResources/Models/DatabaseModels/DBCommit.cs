using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LfkSharedResources.Models.DatabaseModels
{
    [Table(Name = "commits")]
    public class DBCommit
    {
        [Column(Name = "id", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "repository_id")]
        public Guid RepositoryId { get; set; }
        [Column(Name = "index_id")]
        public Guid IndexId { get; set; }
        [Column(Name = "datetime")]
        public DateTime Date { get; set; }
        [Column(Name = "comment")]
        public string Comment { get; set; }

    }
}