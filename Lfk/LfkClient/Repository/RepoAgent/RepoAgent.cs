﻿using System;
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
        private Guid currentIndexId;

        public void InitializeRepoAgent()
        {
            currentIndexId = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile).Id;

            if (currentIndexId == Guid.Empty)
            {
                currentIndexId = Guid.NewGuid();
            }
        }

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

                Index deserializedIndex = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile);

                if (deserializedIndex != null)
                {
                    if (deserializedIndex.Id == Guid.Empty)
                    {
                        deserializedIndex.Id = currentIndexId;
                    }

                    if (deserializedIndex.RepoObjectId_FileName.ContainsValue(fileName))
                    {
                        deserializedIndex.RepoObjectId_FileName.Remove(
                            deserializedIndex.RepoObjectId_FileName.Select(p => p).Where(p => p.Value == fileName).First().Key);                       
                    }

                    deserializedIndex.RepoObjectId_FileName.Add(id, fileName);
                    JsonSerializer.SerializeObjectToFile(deserializedIndex, FileSystemPaths.LfkIndexFile);
                }
            }
        }

        public void HandleCommit(string message)
        {
            Index index = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile);

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
            index.Id = currentIndexId;

            JsonSerializer.SerializeObjectToFile(index, FileSystemPaths.LfkIndexFile);
        }

        public List<Commit> HandleHistory()
        {
            List<Commit> commits = new List<Commit>();

            foreach (string fileName in FileSystem.ReadWorkingDirectoryFiles(FileTypes.SystemCommits))
            {
                commits.Add(JsonDeserializer.DeserializeObjectFromFile<Commit>(fileName));
            }

            commits = commits.OrderBy(c => c.Date).ToList();

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
            return FileSystem.ReadWorkingDirectoryFiles(FileTypes.Client);
        }

        public string[] GetIncludedFiles()
        {
            return JsonDeserializer.DeserializeObjectFromFile<string[]>(FileSystemPaths.LfkIncludedFile);
        }

        public string[] GetUnincludedFiles()
        {
            return GetWorkingDirectoryFiles().Except(GetIncludedFiles()).ToArray();
        }

        public string[] GetChangedFilesAfterLastCommit()
        {
            List<string> changedFilesAfterLastCommit = new List<string>();

            Index currentIndex = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile);

            List<Commit> commits = HandleHistory();
            if (commits.Count == 0)
            {
                changedFilesAfterLastCommit.AddRange(currentIndex.RepoObjectId_FileName.Values);
            }
            else
            {
                Commit lastCommit = HandleHistory().Last();

                foreach (var blobInfo in currentIndex.RepoObjectId_FileName)
                {
                    if (!lastCommit.Index.RepoObjectId_FileName.ContainsKey(blobInfo.Key))
                    {
                        changedFilesAfterLastCommit.Add(blobInfo.Value);
                    }
                }
            }

            return changedFilesAfterLastCommit.ToArray();
        }

        public string[] GetChangedFiles()
        {
            List<string> changedFiles = new List<string>();
            List<string> includedFiles = JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile);

            List<Commit> commits = HandleHistory();
            if (commits.Count != 0)
            {
                Index previousIndex = commits.Last().Index;
                changedFiles = GetChangedFilesAccordingToIndex(previousIndex);
            }
            else
            {
                Index currentIndex = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile);

                if (currentIndex.Id == Guid.Empty)
                {
                    changedFiles.AddRange(includedFiles);
                }
                else
                {
                    changedFiles = GetChangedFilesAccordingToIndex(currentIndex);
                }
            }

            return changedFiles.ToArray();
        }

        private List<string> GetChangedFilesAccordingToIndex(Index index)
        {
            List<string> changedFiles = new List<string>();
            List<string> includedFiles = JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile);

            foreach (string includedFile in includedFiles)
            {
                string fileContent = FileSystem.ReadFileContent(includedFile);

                if (index.RepoObjectId_FileName.ContainsValue(includedFile))
                {
                    Guid previosBlobId = index.RepoObjectId_FileName.First(i => i.Value == includedFile).Key;
                    RepoObject previosBlob = JsonDeserializer.DeserializeObjectFromFile<RepoObject>(FileSystemPaths.LfkObjectsFolder + previosBlobId);
                    string previosFileContent = previosBlob.Hash;

                    if (fileContent != previosFileContent)
                    {
                        changedFiles.Add(includedFile);
                    }
                }
            }

            return changedFiles;
        }

        #endregion
    }
}