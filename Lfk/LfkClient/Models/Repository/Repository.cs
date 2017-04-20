using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LfkClient.Models.Repository
{
    /// <summary>
    /// Модель экземпляра конкретного репозитория
    /// </summary>
    public class Repository : AbstractRepository
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public string Path { get; set; }
    }
}
