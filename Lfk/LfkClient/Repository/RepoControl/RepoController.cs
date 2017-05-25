using System.Collections.Generic;
using System.Linq;
using LfkSharedResources.Models.Repository;
using LfkClient.FileSystemControl;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkActions;
using LfkClient.ServerConnection;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Models;
using System;
using LfkExceptions;

namespace LfkClient.Repository.RepoControl
{
    /// <summary>
    /// Отвечает за обработку команд по управлению репозиторием
    /// </summary>
    internal class RepoController
    {
        static RepoController()
        {
            JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            JsonSerializer.WriteMethod = FileSystem.WriteToFile;
        }
        /// <summary>
        /// Инициализирует системный каталог lfk необходимыми файлами и папками
        /// </summary>
        public bool Create(AbstractRepository abstractRepository, out string message)
        {
            LocalRepository repo = abstractRepository as LocalRepository;
            ServerRepository serverRepository = new ServerRepository()
            {
                LocalRepository = repo,
                Commits = new List<Commit>(),
                Objects = new List<RepoObject>()
            };

            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Create, serverRepository);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            message = responsePackage.OperationInfo.Message;

            if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {
                Initialization(repo);
            }

            return responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok ? true : false;
        }

        /// <summary>
        /// Открывает каталог содержащий репозиторий 
        /// </summary>
        public void OpenLocal(string path, Guid userId)
        {
            FileSystem.Path = path;
        }


        public List<LocalRepository> GetManagedRepositories(string userId)
        {
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.View, userId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            List<LocalRepository> repositories = responsePackage.Data as List<LocalRepository>;
            return repositories;
        }

        public bool Download(string path, string repositoryId, out string message)
        {
            
            message = string.Empty;
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Read, repositoryId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            ServerRepository serverRepository = responsePackage.Data as ServerRepository;

            path +="\\" +  serverRepository.LocalRepository.Title;

            if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {
                serverRepository.LocalRepository.Path = path;

                Initialization(serverRepository.LocalRepository);

                JsonSerializer.SerializeObjectToFile(serverRepository.Files, FileSystemPaths.LfkFilesFile);

                JsonSerializer.SerializeObjectToFile(
                    serverRepository.Files.Select(file => file.Filename).ToList(),
                    FileSystemPaths.LfkIncludedFile);

                foreach (RepoObject repoObject in serverRepository.Objects)
                {
                    string serilaizedRepoObject = JsonSerializer.SerializeObject(repoObject);
                    FileSystem.InitializeInexistentFile(FileSystemPaths.LfkObjectsFolder + repoObject.Id.ToString(), serilaizedRepoObject);
                }

                foreach (Commit commit in serverRepository.Commits)
                {
                    string serializedCommit = JsonSerializer.SerializeObject(commit);
                    FileSystem.InitializeInexistentFile(FileSystemPaths.LfkCommitsFolder + commit.Id.ToString(), serializedCommit);
                }

                foreach (File file in serverRepository.Files)
                {
                    FileSystem.CreateFileWithFolders(file.Filename);
                }
            }

            message = responsePackage.OperationInfo.Message;
            return responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok ? true : false;
        }

        internal bool IsUserHolderOfRepository(string path, Guid userId)
        {
            bool rc = false;
            LocalRepository folderRepository = JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
            if (folderRepository != null)
            {
                rc = folderRepository.UserId == userId;
            }

            return rc;
        }

        internal bool IsExistRepositorySameTitle(LocalRepository localRepository)
        {
            bool rc = false;
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.View, localRepository.UserId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            List<LocalRepository> repositories = responsePackage.Data as List<LocalRepository>;

            if (repositories.Contains(repositories.FirstOrDefault(rep => rep.Title == localRepository.Title)))
            {
                rc = true;
            }

            return rc;
        }

        public void Delete(string repositoryId)
        {
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Delete, repositoryId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            ServerRepository serverRepository = responsePackage.Data as ServerRepository;
            if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {
                FileSystem.DeleteFolder(FileSystemPaths.LfkMainFolder);
            }
            else
            {

            }
        }

        public bool IsFolderContainRepository()
        {
            bool rc = false;
            rc = FileSystem.IsFolderExist(FileSystemPaths.LfkMainFolder);
            return rc;
        }

        private void ClearRepositoryFolder()
        {
            if (FileSystem.IsFolderExist(FileSystemPaths.LfkMainFolder))
            {
                FileSystem.DeleteFolder(FileSystemPaths.LfkMainFolder);
            }
        }

        private void Initialization(LocalRepository repo)
        {
            FileSystem.Path = repo.Path;
            ClearRepositoryFolder();

            FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

            FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
            FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIncludedFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIndexFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkInfoFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkFilesFile, "[]");

            JsonSerializer.SerializeObjectToFile(repo, FileSystemPaths.LfkInfoFile);

            Repository.GetInstance().RepoAgent.InitializeRepoAgent();
        }
    }
}