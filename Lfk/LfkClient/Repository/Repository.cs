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
using LfkClient.UserMessages;
using System.Threading.Tasks;

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
            return RepoController.Create(abstractRepository, out message);
        }

        public void OpenLocal(string path, Guid userId)
        {
            RepoController.OpenLocal(path, userId);
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

        public void Update()
        {
            RepoAgent.HandleUpdate();
        }

        public void Switch(Commit commit)
        {
            RepoAgent.HandleSwitch(commit);
        }

        public InvalidCommitSwitchingReasons CanSwitch(Commit commit)
        {
            InvalidCommitSwitchingReasons reason = InvalidCommitSwitchingReasons.None;

            if (RepoAgent.GetChangedFiles().Result.Length != 0 && RepoAgent.GetChangedFilesAfterLastCommit().Result.Length != 0)
            {
                reason = InvalidCommitSwitchingReasons.NotCommittedChanges;
            }

            return reason;
        }

        public void Uninclude(IEnumerable<string> unincluded)
        {
            RepoAgent.HandleUninclude(unincluded);
        }

        public void Reset(IEnumerable<string> reseted)
        {
            RepoAgent.HandleReset(reseted);
        }

        public void Delete(string repositoryId)
        {
            RepoController.Delete(repositoryId);
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

        public async Task<string[]> GetChangedFilesAfterParentCommit()
        {
            return await RepoAgent.GetChangedFilesAfterParentCommit();
        }

        public async Task<string[]> GetChangedFilesAfterLastCommit()
        {
            return await RepoAgent.GetChangedFilesAfterLastCommit();
        }

        /// <summary>
        /// Возвращает список всех изменных файлов после последней команды Add
        /// </summary>
        /// <returns>Список изменных файлов</returns>
        public async Task<string[]> GetChangedFiles()
        {
            return await RepoAgent.GetChangedFiles();
        }

        public List<LocalRepository> GetManagedRepositories(string userId)
        {
            return RepoController.GetManagedRepositories(userId);
        }

        public bool Download(string path, string repositoryId, out string message)
        {
            bool rc = RepoController.Download(path, repositoryId, out message);
            Commit lastCommit = History().LastOrDefault();

            if (lastCommit != null)
            {
                Switch(lastCommit);
            }

            return rc;
        }

        public InvalidRepositoryDownloadReasons CanDownloadRepository(string path)
        {
            FileSystem.Path = path;
            InvalidRepositoryDownloadReasons reason = InvalidRepositoryDownloadReasons.None;

            if (FileSystem.IsFolderExist(path))
            {
                
                reason = InvalidRepositoryDownloadReasons.FolderAlreadyContainsRepository;
            }

            return reason;
        }

        public string GetCurrentRepositoryName()
        {
            return RepoAgent.GetCurrentRepositoryPath();
        }

        public InvalidRepositoryCreationReasons CanCreateRepository(LocalRepository localRepository)
        {
            FileSystem.Path = localRepository.Path;
            InvalidRepositoryCreationReasons reason = InvalidRepositoryCreationReasons.None;

            if (RepoController.IsFolderContainRepository())
            {
                reason = InvalidRepositoryCreationReasons.FolderAlreadyContainsRepository;
            }
            else if (RepoController.IsExistRepositorySameTitle(localRepository))
            {
                reason = InvalidRepositoryCreationReasons.DuplicateTitle;
            }

            return reason;
        }

        public InvalidRepositoryOpenReasons CanOpenRepository(string path, Guid userid)
        {
            FileSystem.Path = path;
            InvalidRepositoryOpenReasons reason = InvalidRepositoryOpenReasons.None;

            if (!RepoController.IsFolderContainRepository())
            {
                reason = InvalidRepositoryOpenReasons.FolderDoesNotContainRepository;
            }
            else if (!RepoController.IsUserHolderOfRepository(path,userid))
            {
                reason = InvalidRepositoryOpenReasons.RepositoryDoNotBelongToUser;
            }

            return reason;
        }
    }
}