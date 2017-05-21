using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkClient.Repository.RepoAgent;
using LfkClient.Repository.RepoControl;
using LfkSharedResources.Models.Repository;
using LfkClient.FileSystemControl;
using LfkSharedResources.Models;

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

        public bool TryInit(AbstractRepository abstractRepository, out string message)
        {
            return RepoController.Init(abstractRepository, out message);
        }

        public void OpenLocal(string path)
        {
            RepoController.OpenLocal(path);
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

        public void Upload()
        {
            RepoController.Upload();
        }

        public void Switch(Commit commit)
        {
            RepoAgent.HandleSwitch(commit);
        }

        public void Uninclude(IEnumerable<string> unincluded)
        {
            RepoAgent.HandleUninclude(unincluded);
        }

        public void Reset(IEnumerable<string> reseted)
        {
            RepoAgent.HandleReset(reseted);
        }

        public string[] GetWorkingDirectoryFiles()
        {
            return RepoAgent.GetWorkingDirectoryFiles();
        }

        public string[] GetIncludedFiles()
        {
            return RepoAgent.GetIncludedFiles();
        }

        public string[] GetUnincludedFiles()
        {
            return RepoAgent.GetUnincludedFiles();
        }

        public string[] GetChangedFilesAfterLastCommit()
        {
            return RepoAgent.GetChangedFilesAfterLastCommit();
        }

        /// <summary>
        /// Возвращает список всех изменных файлов после последней команды Add
        /// </summary>
        /// <returns>Список изменных файлов</returns>
        public string[] GetChangedFiles()
        {
            return RepoAgent.GetChangedFiles();
        }

        public List<LocalRepository> GetManagedRepositories(string userId)
        {
            return RepoController.GetManagedRepositories(userId);
        }

        public void Download(string repositoryId)
        {
            RepoController.Download(repositoryId);
        }
    }
}