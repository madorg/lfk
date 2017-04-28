using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region Методы обработки входящих команд
        public void HandleInclude(IEnumerable<string> included)
        {
            List<string> deserializedOldData =
                JsonDeserializer.DeserializeObjectFromFile<List<string>>(@"\lfk\included.json");

            if (deserializedOldData != null)
            {
                deserializedOldData.AddRange(included);
                JsonSerializer.SerializeObjectToFile(deserializedOldData, @"\lfk\included.json");
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

                FileSystem.CreateFile(@"\lfk\objects\" + blobFileName);
                FileSystem.WriteToFile(@"\lfk\objects\" + blobFileName, serizalizedBlob);

                Dictionary<Guid, string> deserializedIndex =
                    JsonDeserializer.DeserializeObjectFromFile<Dictionary<Guid, string>>(@"\lfk\index.json");

                if (deserializedIndex != null)
                {
                    deserializedIndex.Add(id, fileName);
                    JsonSerializer.SerializeObjectToFile(deserializedIndex, @"\lfk\index.json");
                }
            }
        }

        public void HandleCommit(string message)
        {
            Index index = new Index()
            {
                Id = currentIndexId,
                RepoObjectId_FileName =
                    JsonDeserializer.DeserializeObjectFromFile<Dictionary<Guid, string>>(@"\lfk\index.json")
            };

            Commit commit = new Commit()
            {
                Id = Guid.NewGuid(),
                Index = index,
                Date = DateTime.Now,
                Comment = message,
                Parent = null
            };

            JsonSerializer.SerializeObjectToFile(commit, @"\lfk\commits\" + commit.Id.ToString());

            currentIndexId = Guid.NewGuid();
        }

        public List<Commit> HandleHistory()
        {
            List<Commit> commits = new List<Commit>();

            foreach (string fileName in FileSystem.ReadWorkingDirectory(FilesType.SystemCommits))
            {
                commits.Add(JsonDeserializer.DeserializeObjectFromFile<Commit>(fileName));
            }

            return commits;
        }

        public void HandleUpload()
        {

        }

        public void HandleSwitch(string commitId)
        {

        }

        #endregion
    }
}