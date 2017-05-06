using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkSharedResources.Models.Repository
{
    /// <summary>
    /// Модель экземпляра конкретного репозитория
    /// </summary>
    public class LocalRepository : AbstractRepository
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public string Path { get; set; }
    }
}
