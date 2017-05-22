using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Models;

namespace LfkSharedResources.Models.Repository
{
    public class ServerRepository : AbstractRepository
    {
        public LocalRepository LocalRepository { get; set; }
        public List<Commit> Commits { get; set; }
        public List<RepoObject> Objects { get; set; }
        public List<File> Files { get; set; }
    }
}