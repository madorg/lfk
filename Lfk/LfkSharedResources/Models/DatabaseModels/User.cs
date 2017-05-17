using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LfkSharedResources.Models.DatabaseModels
{
    [Table(Name = "users")]
    public class DBUser
    {
        [Column(Name = "id", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "name")]
        public string Name { get; set; }
        [Column(Name = "email")]
        public string Email { get; set; }
        [Column(Name = "password")]
        public string Password { get; set; }
    }
}