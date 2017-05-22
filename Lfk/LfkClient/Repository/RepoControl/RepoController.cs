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
                try
                {
                    Initialization(repo);
                }
                catch (FolderAlreadyContainRepositoryException facre)
                {
                    message = facre.Message;
                    responsePackage.OperationInfo.Code = NetworkStatusCodes.Fail;
                }
            }
          
            return responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok ? true : false;
        }

        /// <summary>
        /// Открывает каталог содержащий репозиторий 
        /// </summary>
        public void OpenLocal(string path,Guid userId)
        {
            FileSystem.Path = path;
            try
            {
                LocalRepository repo = JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
                if (repo.UserId != userId)
                    throw new NotAllowedOpenRepository("Извините, но это не ваш репозиторий");
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Если файла не существует то обрабатывается исключение и дальше выбрасывается с информацией о том что не найден файл инициализации
                throw;
            }
        }


        public List<LocalRepository> GetManagedRepositories(string userId)
        {
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.View, userId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            List<LocalRepository> repositories = responsePackage.Data as List<LocalRepository>;
            return repositories;
        }

        public bool Download(string path, string repositoryId,out string message)
        {
            message = string.Empty;
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Read, repositoryId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            ServerRepository serverRepository = responsePackage.Data as ServerRepository;

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
                    FileSystem.InitializeInexistentFile(FileSystemPaths.LfkObjectsFolder +  repoObject.Id.ToString(), serilaizedRepoObject);
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

        public void Delete(string repositoryId)
        {
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Delete, repositoryId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            ServerRepository serverRepository = responsePackage.Data as ServerRepository;
            if(responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {

            }
            else
            {

            }

        }
        private void Initialization(LocalRepository repo)
        {
            FileSystem.Path = repo.Path;
            JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            JsonSerializer.WriteMethod = FileSystem.WriteToFile;
            LocalRepository localRepository = null;

            try
            {
                localRepository = JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
                if (localRepository != null)
                {
                    throw new FolderAlreadyContainRepositoryException("В папке " + repo.Path + " уже содержится репозиторий.\n Вы уверены что он вам безразличен?");
                }
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {

            }

           
            FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

            FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
            FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIncludedFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIndexFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkInfoFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkFilesFile, "[]");


            JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            JsonSerializer.WriteMethod = FileSystem.WriteToFile;

            JsonSerializer.SerializeObjectToFile(repo, FileSystemPaths.LfkInfoFile);

            Repository.GetInstance().RepoAgent.InitializeRepoAgent();
        }
    }
}