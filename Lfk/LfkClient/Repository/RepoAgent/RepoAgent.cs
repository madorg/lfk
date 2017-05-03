using System;
using System.Linq;
using System.Collections.Generic;
using LfkClient.FileSystemControl;
using LfkClient.Serialization.Json;
using LfkClient.Models;

namespace LfkClient.Repository.RepoAgent
{
    /// <summary>
    /// Отвечает за обработку команд по работе с содержимым репозитория
    /// </summary>
    internal class RepoAgent
    {
        private static Guid currentIndexId = Guid.NewGuid();

        #region Обработчики основных команд системы контроля версия (Handle)

        public void HandleInclude(IEnumerable<string> included)
        {
            List<string> deserializedOldData =
                JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile);

            if (deserializedOldData != null)
            {
                deserializedOldData.AddRange(included);
                JsonSerializer.SerializeObjectToFile(deserializedOldData, FileSystemPaths.LfkIncludedFile);
            }
        }

        public void HandleAdd(IEnumerable<string> added)
        {
            foreach (string fileName in added)
            {
                File file = new File() { Id = Guid.NewGuid(), Filename = fileName };

                Guid id = Guid.NewGuid();
                string hash = FileSystem.ReadFileContent(fileName);
                Guid fileId = file.Id;
                Guid indexId = currentIndexId;

                RepoObject blob = new RepoObject() { Id = id, Hash = hash, FileId = fileId, IndexId = indexId };
                string serizalizedBlob = JsonSerializer.SerializeObject(blob);

                string blobFileName = id.ToString();

                FileSystem.CreateFile(FileSystemPaths.LfkObjectsFolder + blobFileName);
                FileSystem.WriteToFile(FileSystemPaths.LfkObjectsFolder + blobFileName, serizalizedBlob);

                Dictionary<Guid, string> deserializedIndex =
                    JsonDeserializer.DeserializeObjectFromFile<Dictionary<Guid, string>>(FileSystemPaths.LfkIndexFile);

                if (deserializedIndex != null)
                {
                    if (deserializedIndex.ContainsValue(fileName))
                    {
                        deserializedIndex.Remove(
                            deserializedIndex.Select(p => p).Where(p => p.Value == fileName).First().Key);                       
                    }

                    deserializedIndex.Add(id, fileName);

                    JsonSerializer.SerializeObjectToFile(deserializedIndex, FileSystemPaths.LfkIndexFile);
                }
            }
        }

        public void HandleCommit(string message)
        {
            Index index = new Index()
            {
                Id = currentIndexId,
                RepoObjectId_FileName =
                    JsonDeserializer.DeserializeObjectFromFile<Dictionary<Guid, string>>(FileSystemPaths.LfkIndexFile)
            };

            Commit commit = new Commit()
            {
                Id = Guid.NewGuid(),
                Index = index,
                Date = DateTime.Now,
                Comment = message,
                Parent = null
            };

            JsonSerializer.SerializeObjectToFile(commit, FileSystemPaths.LfkCommitsFolder + commit.Id.ToString());

            currentIndexId = Guid.NewGuid();
        }

        public List<Commit> HandleHistory()
        {
            List<Commit> commits = new List<Commit>();

            foreach (string fileName in FileSystem.ReadWorkingDirectory(FilesType.SystemCommits))
            {
                commits.Add(JsonDeserializer.DeserializeObjectFromFile<Commit>(fileName));
            }

            commits.OrderBy(c => c.Date);

            return commits;
        }

        public void HandleUpload()
        {

        }

        /// <summary>
        /// Обеспечивает переключение состояний файлов в соответствии с указанным коммитом
        /// </summary>
        /// <param name="commit">Объект коммита, на который обеспечивается переключение</param>
        public void HandleSwitch(Commit commit)
        {
            foreach (KeyValuePair<Guid, string> idFileNamePair in commit.Index.RepoObjectId_FileName)
            {
                RepoObject oldBlob = JsonDeserializer.DeserializeObjectFromFile<RepoObject>(
                    FileSystemPaths.LfkObjectsFolder + idFileNamePair.Key.ToString());

                FileSystem.WriteToFile(idFileNamePair.Value, oldBlob.Hash);
            }
        }

        public void HandleUninclude(IEnumerable<string> unincluded)
        {
            List<string> included = JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile);

            foreach (string unincludedItem in unincluded)
            {
                included.Remove(unincludedItem);
            }

            JsonSerializer.SerializeObjectToFile(included, FileSystemPaths.LfkIncludedFile);
        }

        #endregion

        #region Обработчики служебных команд (Get)

        public string[] GetWorkingDirectoryFiles()
        {
            return FileSystem.ReadWorkingDirectory(FilesType.Client);
        }

        public string[] GetIncludedFiles()
        {
            return JsonDeserializer.DeserializeObjectFromFile<string[]>(FileSystemPaths.LfkIncludedFile);
        }

        public string[] GetChangedFilesAfterLastCommit()
        {
            Commit lastCommit = HandleHistory().Last();

            Dictionary<Guid, string> currentIndex =
                    JsonDeserializer.DeserializeObjectFromFile<Dictionary<Guid, string>>(FileSystemPaths.LfkIndexFile);

            List<string> changedFilesAfterLastCommit = new List<string>();
            foreach (var blobInfo in currentIndex)
            {
                if (!lastCommit.Index.RepoObjectId_FileName.ContainsKey(blobInfo.Key))
                {
                    changedFilesAfterLastCommit.Add(blobInfo.Value);
                }
            }

            return changedFilesAfterLastCommit.ToArray();
        }

        #endregion
    }
}