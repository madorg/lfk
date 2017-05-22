using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using LfkSharedResources.Models.Repository;

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

        public static explicit operator LocalRepository(DBRepository repo)
        {
            LocalRepository lr = new LocalRepository()
            {
                Id = repo.Id,
                Title = repo.Title,
                UserId = repo.UserId,
                Path = string.Empty
            };
            return lr;
        }
    }
}