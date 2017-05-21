using System.Collections.Generic;
using LfkSharedResources.Models.Repository;
using LfkClient.FileSystemControl;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkActions;
using LfkClient.ServerConnection;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Models;

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
        public bool Init(AbstractRepository abstractRepository, out string message)
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

            if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {
                FileSystem.Path = repo.Path;

                FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

                FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
                FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

                FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIncludedFile, "[]");
                FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIndexFile, "{}");
                FileSystem.InitializeInexistentFile(FileSystemPaths.LfkInfoFile, "{}");

                JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
                JsonSerializer.WriteMethod = FileSystem.WriteToFile;

                JsonSerializer.SerializeObjectToFile(repo, FileSystemPaths.LfkInfoFile);

                Repository.GetInstance().RepoAgent.InitializeRepoAgent();
            }

            message = responsePackage.OperationInfo.Message;
            return responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok ? true : false;
        }

        /// <summary>
        /// Открывает каталог содержащий репозиторий 
        /// </summary>
        public void OpenLocal(string path)
        {
            FileSystem.Path = path;
            JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            JsonSerializer.WriteMethod = FileSystem.WriteToFile;

            try
            {
                LocalRepository repo = JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Если файла не существует то обрабатывается исключение и дальше выбрасывается с информацией о том что не найден файл инициализации
                throw;
            }
        }

        public void Upload()
        {
            LocalRepository localRepository = JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
            List<Commit> commits = Repository.GetInstance().History();
            List<RepoObject> objects = new List<RepoObject>();
            foreach (string filename in FileSystem.ReadWorkingDirectoryFiles(FileTypes.SystemObjects))
            {
                RepoObject repoObject = JsonDeserializer.DeserializeObjectFromFile<RepoObject>(filename);
                objects.Add(repoObject);
            }

            ServerRepository serverRepository = new ServerRepository()
            {
                LocalRepository = localRepository,
                Commits = commits,
                Objects = objects
            };

            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Create, serverRepository);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);

            System.Windows.Forms.MessageBox.Show(responsePackage.OperationInfo.Message);
        }

        public List<LocalRepository> GetManagedRepositories(string userId)
        {
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.View, userId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            List<LocalRepository> repositories = responsePackage.Data as List<LocalRepository>;
            return repositories;
        }

        public void Download(string repositoryId)
        {
            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Read, repositoryId);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);
            ServerRepository serverRepository = responsePackage.Data as ServerRepository;
            ////////////
        }
    }
}   