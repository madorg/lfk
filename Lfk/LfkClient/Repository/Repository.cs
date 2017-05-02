using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkClient.Repository.RepoAgent;
using LfkClient.Repository.RepoControl;
using LfkClient.Models.Repository;
using LfkClient.FileSystemControl;
using LfkClient.Models;

namespace LfkClient.Repository
{
    /// <summary>
    /// Фасад репозитория, перенаправляющий запросы из формы в соответствующие обработчики
    /// </summary>
    public class Repository
    {
        private static Repository repository;

        internal RepoAgent.RepoAgent RepoAgent { get; set; }
        internal RepoController RepoController { get; set; }

        private Repository()
        {
            RepoAgent = new RepoAgent.RepoAgent();
            RepoController = new RepoController();          
        }

        public static Repository GetInstance()
        {
            if (repository == null)
            {
                repository = new Repository();
            }

            return repository;
        }

        public void Init(AbstractRepository abstractRepository)
        {
            RepoController.Init(abstractRepository);
        }

        public void Include(IEnumerable<string> included)
        {
            RepoAgent.HandleInclude(included);
        }

        public void Add(IEnumerable<string> added)
        {
            RepoAgent.HandleAdd(added);
        }

        public void Commit(string message)
        {
            RepoAgent.HandleCommit(message);
        }

        public List<Commit> History()
        {
            return RepoAgent.HandleHistory();
        }

        public void Switch(Commit commit)
        {
            RepoAgent.HandleSwitch(commit);
        }

        public void Uninclude(IEnumerable<string> unincluded)
        {
            RepoAgent.HandleUninclude(unincluded);
        }

        public string[] GetWorkingDirectoryFiles()
        {
            return FileSystem.ReadWorkingDirectory(FilesType.Client);
        }
    }
}