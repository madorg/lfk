using LfkSharedResources.Models.Repository;
using LfkClient.FileSystemControl;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkActions;
using LfkClient.ServerConnection;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Serialization.Json;

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

            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Create, repo);
            NetworkOperationInfo responseInfo = ServerConnector.Create(data);

            if (responseInfo.Code == NetworkStatusCodes.Ok)
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
            else if(responseInfo.Code == NetworkStatusCodes.Fail)
            {
                //Очень опасно !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                throw new System.Exception(responseInfo.Message);
            }
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
    }
}