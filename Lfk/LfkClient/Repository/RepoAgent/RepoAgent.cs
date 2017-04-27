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
            string oldData = FileSystem.ReadFileContent(@"\lfk\included.json");
            List<string> deserializedOldData = 
                JsonDeserializer.DeserializeObject<List<string>>(oldData) as List<string>;

            if (deserializedOldData != null)
            {
                deserializedOldData.AddRange(included);
                string newData = JsonSerializer.SerializeObject(deserializedOldData);
                FileSystem.WriteToFile(@"\lfk\included.json", newData);
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

                string blobFileName = hash.Substring(0, 9);

                // ДЕЗЕЛИАЛИЗОВАТЬ index.json , ДОБАВТЬ ТУДА ЗНАЧЕНИЯ НОВЫЕ (filename - hash_id)
                // СЕРИАЛИЗОВАТЬ И СОХРАНИТЬ ОБРАТНО 

                FileSystem.CreateFile(@"\lfk\objects\" + blobFileName);
                FileSystem.WriteToFile(@"\lfk\objects\" + blobFileName, serizalizedBlob);
            }
        }

        public void HandleCommit(string message)
        {
            Index index = new Index();
            


            Commit commit = new Commit()
            {
                Id = Guid.NewGuid(),
                

            };
        }

        public void HandleHistory()
        {

        }

        public void HandleUpload()
        {

        }

        public void HandleSwitch()
        {

        }

        #endregion
    }
}