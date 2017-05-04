using LfkClient.Models.Repository;
using LfkClient.FileSystemControl;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkActions;
using LfkClient.ServerConnection;

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
        public void Init(AbstractRepository abstractRepository)
        {
            LocalRepository repo = abstractRepository as LocalRepository;

            NetworkPackageController npc = new NetworkPackageController();
            byte[] data = npc.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Create, repo);

            // Проверка ответа сервера
            ServerConnector.Create(data);

            FileSystem.Path = repo.Path;

            FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

            FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
            FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkFilesFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIncludedFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIndexFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkInfoFile, "");

            Serialization.Json.JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            Serialization.Json.JsonSerializer.WriteMethod = FileSystem.WriteToFile;

            Repository.GetInstance().RepoAgent.InitializeRepoAgent();
        }
    }
}