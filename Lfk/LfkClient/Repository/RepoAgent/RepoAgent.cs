﻿using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LfkClient.FileSystemControl;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Models;
using LfkSharedResources.Coding.HuffmanCoding;
using LfkSharedResources.Models.Repository;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkActions;
using LfkClient.ServerConnection;
using LfkSharedResources.Networking.NetworkPackages;

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
            Index index = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile);

            if (index.Id == Guid.Empty)
            {
                index.Id = Guid.NewGuid();
                currentIndexId = index.Id;
                JsonSerializer.SerializeObjectToFile<Index>(index, FileSystemPaths.LfkIndexFile);
            }
            
        }

        #region Обработчики основных команд системы контроля версия (Handle)

        public void HandleInclude(IEnumerable<string> included)
        {
            List<string> deserializedIncludedFile =
                JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile);

            deserializedIncludedFile.AddRange(included);
            List<File> files = JsonDeserializer.DeserializeObjectFromFile<List<File>>(FileSystemPaths.LfkFilesFile);
            foreach (string filename in included)
            {
                files.Add(new File()
                {
                    Id = Guid.NewGuid(),
                    Filename = filename
                });
            }
            JsonSerializer.SerializeObjectToFile(files, FileSystemPaths.LfkFilesFile);
            JsonSerializer.SerializeObjectToFile(deserializedIncludedFile, FileSystemPaths.LfkIncludedFile);
        }

        public void HandleAdd(IEnumerable<string> added)
        {
            List<File> deserializedFiles = JsonDeserializer.DeserializeObjectFromFile<List<File>>(FileSystemPaths.LfkFilesFile);
            foreach (string fileName in added)
            {
                Guid id = Guid.NewGuid();

                string addedFileContent = FileSystem.ReadFileContent(fileName);

                HuffmanTree huffmanTree = new HuffmanTree(addedFileContent);
                byte[] encodedFileContent = huffmanTree.EncodeData(addedFileContent);
                byte[] encodedHuffmanTree = huffmanTree.EncodeHuffmanTree();

                Guid indexId = currentIndexId;
                Guid fileId = deserializedFiles.Find(f => f.Filename == fileName).Id;
                RepoObject blob = new RepoObject()
                {
                    Id = id,
                    IndexId = indexId,
                    FileId = fileId,
                    Hash = encodedFileContent,
                    HuffmanTree = encodedHuffmanTree
                };
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

                    if (deserializedIndex.RepoObjectIdAndFileName.ContainsValue(fileName))
                    {
                        deserializedIndex.RepoObjectIdAndFileName.Remove(
                            deserializedIndex.RepoObjectIdAndFileName.Select(p => p).Where(p => p.Value == fileName).First().Key);
                    }

                    deserializedIndex.RepoObjectIdAndFileName.Add(id, fileName);
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
                Comment = message
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

        /// <summary>
        /// Обеспечивает переключение состояний файлов в соответствии с указанным коммитом
        /// </summary>
        /// <param name="commit">Объект коммита, на который обеспечивается переключение</param>
        public void HandleSwitch(Commit commit)
        {
            foreach (KeyValuePair<Guid, string> idFileNamePair in commit.Index.RepoObjectIdAndFileName)
            {
                RepoObject oldBlob = JsonDeserializer.DeserializeObjectFromFile<RepoObject>(
                    FileSystemPaths.LfkObjectsFolder + idFileNamePair.Key.ToString());

                HuffmanTree huffmanTree = new HuffmanTree(oldBlob.HuffmanTree);
                string decodedFileContent = huffmanTree.DecodeData(oldBlob.Hash);
                //FileSystem.CreateFile(idFileNamePair.Value);

                FileSystem.CreateFileWithFolders(idFileNamePair.Value);
                FileSystem.WriteToFile(idFileNamePair.Value, decodedFileContent);
            }
        }

        public void HandleUninclude(IEnumerable<string> unincluded)
        {
            List<string> included = JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile);
            List<File> files = JsonDeserializer.DeserializeObjectFromFile<List<File>>(FileSystemPaths.LfkFilesFile);
            foreach (string unincludedItem in unincluded)
            {
                included.Remove(unincludedItem);
                files.Remove(files.FirstOrDefault(f => f.Filename == unincludedItem));
            }
            JsonSerializer.SerializeObjectToFile(files, FileSystemPaths.LfkFilesFile);
            JsonSerializer.SerializeObjectToFile(included, FileSystemPaths.LfkIncludedFile);
        }

        /// <summary>
        /// Обеспечивает удаление ошибочно созданного блоба
        /// </summary>
        public void HandleReset(IEnumerable<string> reseted)
        {
            Index deserializedIndex = JsonDeserializer.DeserializeObjectFromFile<Index>(FileSystemPaths.LfkIndexFile);

            foreach (string fileName in reseted)
            {
                Guid blobId = deserializedIndex.RepoObjectIdAndFileName.First(repoObject => repoObject.Value == fileName).Key;
                FileSystem.DeleteFile(FileSystemPaths.LfkObjectsFolder + blobId.ToString());
                deserializedIndex.RepoObjectIdAndFileName.Remove(blobId);

                Commit previousCommit = HandleHistory().LastOrDefault();

                if (previousCommit != null)
                {
                    Index previousIndex = previousCommit.Index;

                    var previousBlobInfo = previousIndex.RepoObjectIdAndFileName.SingleOrDefault(repoObject => repoObject.Value == fileName);
                    if (previousBlobInfo.Key != Guid.Empty)
                    {
                        deserializedIndex.RepoObjectIdAndFileName.Add(previousBlobInfo.Key, previousBlobInfo.Value);
                    }
                }

                JsonSerializer.SerializeObjectToFile(deserializedIndex, FileSystemPaths.LfkIndexFile);
            }
        }

        public void HandleUpdate()
        {
            LocalRepository localRepository = JsonDeserializer.DeserializeObjectFromFile<LocalRepository>(FileSystemPaths.LfkInfoFile);
            List<Commit> commits = Repository.GetInstance().History();
            List<RepoObject> objects = new List<RepoObject>();
            List<File> files = JsonDeserializer.DeserializeObjectFromFile<List<File>>(FileSystemPaths.LfkFilesFile);
            foreach (string filename in FileSystem.ReadWorkingDirectoryFiles(FileTypes.SystemObjects))
            {
                RepoObject repoObject = JsonDeserializer.DeserializeObjectFromFile<RepoObject>(filename);
                objects.Add(repoObject);
            }

            ServerRepository serverRepository = new ServerRepository()
            {
                LocalRepository = localRepository,
                Commits = commits,
                Objects = objects,
                Files = files
            };

            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.Repository, RepositoryNetworkActions.Update, serverRepository);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);

            System.Windows.Forms.MessageBox.Show(responsePackage.OperationInfo.Message);
        }

        #endregion

        #region Обработчики служебных команд (Get)

        public string[] GetWorkingDirectoryFiles()
        {
            return FileSystem.ReadWorkingDirectoryFiles(FileTypes.Client);
        }

        public string[] GetIncludedFiles()
        {
            return JsonDeserializer.DeserializeObjectFromFile<List<string>>(FileSystemPaths.LfkIncludedFile).ToArray();
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
                changedFilesAfterLastCommit.AddRange(currentIndex.RepoObjectIdAndFileName.Values);
            }
            else
            {
                Commit lastCommit = HandleHistory().Last();

                foreach (var blobInfo in currentIndex.RepoObjectIdAndFileName)
                {
                    if (!lastCommit.Index.RepoObjectIdAndFileName.ContainsKey(blobInfo.Key))
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

                if (index.RepoObjectIdAndFileName.ContainsValue(includedFile))
                {
                    Guid previosBlobId = index.RepoObjectIdAndFileName.First(i => i.Value == includedFile).Key;
                    RepoObject previosBlob = JsonDeserializer.DeserializeObjectFromFile<RepoObject>(FileSystemPaths.LfkObjectsFolder + previosBlobId);

                    HuffmanTree huffmanTree = new HuffmanTree(previosBlob.HuffmanTree);
                    string previousFileContent = huffmanTree.DecodeData(previosBlob.Hash);

                    if (fileContent != previousFileContent)
                    {
                        changedFiles.Add(includedFile);
                    }
                }
                else
                {
                    changedFiles.Add(includedFile);
                }
            }

            return changedFiles;
        }

        #endregion
    }
}