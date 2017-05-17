using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LfkSharedResources.Models.DatabaseModels
{
    [Table(Name = "repositories")]
    public class DBRepository
    {
        [Column(Name = "id", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "title")]
        public string Title { get; set; }
        [Column(Name = "user_id")]
        public Guid UserId { get; set; }
    }
}