using LfkClient.Models.Repository;
using LfkClient.FileSystemControl;

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

            // ТУДУ: проверить наличие схожего репо этого же юзера на сервере

            FileSystem.Path = repo.Path;

            FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

            FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
            FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

            FileSystem.CreateFile(FileSystemPaths.LfkFilesFile);
            FileSystem.CreateFile(FileSystemPaths.LfkIncludedFile);
            FileSystem.CreateFile(FileSystemPaths.LfkIndexFile);
            FileSystem.CreateFile(FileSystemPaths.LfkInfoFile);

            FileSystem.AppendToFile(FileSystemPaths.LfkFilesFile, "[]");
            FileSystem.AppendToFile(FileSystemPaths.LfkIncludedFile, "[]");           
            FileSystem.AppendToFile(FileSystemPaths.LfkIndexFile, "{}");
        }
    }
}