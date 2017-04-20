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
            Models.Repository.Repository repo = abstractRepository as Models.Repository.Repository;

            // ТУДУ: проверить наличие схожего репо этого же юзера на сервере

            FileSystemWriter fsw = new FileSystemWriter(repo.Path);

            fsw.CreateFolder("lfk");

            fsw.CreateFolder("lfk/objects");
            fsw.CreateFolder("lfk/commits");
            fsw.CreateFile("lfk/info.json");
            fsw.CreateFile("lfk/index.json");
            fsw.CreateFile("lfk/included.json");
        }
    }
}
