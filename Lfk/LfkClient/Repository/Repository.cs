using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkClient.Repository.RepoAgent;
using LfkClient.Repository.RepoControl;
using LfkClient.Models.Repository;

namespace LfkClient.Repository
{
    public class Repository
    {
        internal RepoAgent.RepoAgent RepoAgent { get; set; }
        internal RepoController RepoController { get; set; }

        public Repository()
        {
            RepoAgent = new RepoAgent.RepoAgent();
            RepoController = new RepoController();
            
        }

        public void Init(AbstractRepository abstractRepository)
        {
            RepoController.Init(abstractRepository);
        }

        public void Include(IEnumerable<string> included)
        {
            RepoAgent.HandleInclude(included);
        }
    }
}