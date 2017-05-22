using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LfkSharedResources.Models.DatabaseModels
{
    [Table(Name = "files")]
    public class DBFile
    {
        [Column(Name = "id", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "filename")]
        public string Filename { get; set; }

        public override bool Equals(object obj)
        {
            DBFile file = obj as DBFile;
            if (file == null)
            {
                return false;
            }
            else
            {
                return Id == file.Id;
            }
        }
    }
}