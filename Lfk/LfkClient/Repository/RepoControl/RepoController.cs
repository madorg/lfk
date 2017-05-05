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

            //NetworkPackageController npc = new NetworkPackageController();
            //byte[] data = npc.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Create, repo);

            // TODO: Проверка ответа сервера
            //ServerConnector.Create(data);
            FileSystem.Path = repo.Path;

            FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

            FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
            FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkFilesFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIncludedFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIndexFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkInfoFile, "{}");

            Serialization.Json.JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            Serialization.Json.JsonSerializer.WriteMethod = FileSystem.WriteToFile;

            Serialization.Json.JsonSerializer.SerializeObjectToFile(repo, FileSystemPaths.LfkInfoFile);

            Repository.GetInstance().RepoAgent.InitializeRepoAgent();
        }

        /// <summary>
        /// Открывает каталог содержащий репозиторий 
        /// </summary>
        public void OpenLocal(string path)
        {
            FileSystem.Path = path;
            Serialization.Json.JsonDeserializer.ReadMethod = FileSystem.ReadFileContent;
            Serialization.Json.JsonSerializer.WriteMethod = FileSystem.WriteToFile;
            try
            {
                LocalRepository repo = Serialization.Json.JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Если файла не существует то обрабатывается исключение и дальше выбрасывается с информацией о том что не найден файл инициализации
                throw;
            }
        }
    }
}