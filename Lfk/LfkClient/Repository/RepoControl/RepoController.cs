﻿using LfkClient.Models.Repository;
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

            // TODO: проверить наличие схожего репо этого же юзера на сервере

            FileSystem.Path = repo.Path;

            FileSystem.CreateFolder(FileSystemPaths.LfkMainFolder);

            FileSystem.CreateFolder(FileSystemPaths.LfkObjectsFolder);
            FileSystem.CreateFolder(FileSystemPaths.LfkCommitsFolder);

            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkFilesFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIncludedFile, "[]");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkIndexFile, "{}");
            FileSystem.InitializeInexistentFile(FileSystemPaths.LfkInfoFile, "");
        }
    }
}