using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkClient.Models.Repository;
using LfkClient.FileSystemControl;

namespace LfkClient.Repository.RepoControl
{
    /// <summary>
    /// Отвечает за обработку команд по управлению репозиторием
    /// </summary>
    internal class RepoController
    {
        // Структура каталога lfk:
        // - commits/
        // --- <commits>
        // - objects/ 
        // --- <blobs>
        // - info
        // - index
        // - included

        public void Init(AbstractRepository abstractRepository)
        {
            LocalRepository repo = abstractRepository as LocalRepository;

            // ТУДУ: проверить наличие схожего репо этого же юзера на сервере

            FileSystem.Path = repo.Path;

            FileSystem.CreateFolder("/lfk");

            FileSystem.CreateFolder("/lfk/objects");
            FileSystem.CreateFolder("/lfk/commits");
            FileSystem.CreateFile("/lfk/info.json");
            FileSystem.CreateFile("/lfk/index.json");
            FileSystem.CreateFile("/lfk/included.json");
            FileSystem.CreateFile("/lfk/files.json");

            FileSystem.AppendToFile(@"\lfk\included.json", "[]");
            FileSystem.AppendToFile(@"\lfk\files.json", "[]");
            FileSystem.AppendToFile(@"\lfk\index.json", "{}");
        }
    }
}