using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LfkClient.FileSystemControl;

namespace LfkClient.Serialization.Json
{
    class JsonDeserializer
    {
        public static T DeserializeObjectFromFile<T>(string fileName)
        {
            string oldData = FileSystem.ReadFileContent(fileName);
            T deserializedOldData = DeserializeObject<T>(oldData);
            return deserializedOldData;
        }

        public static T DeserializeObject<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}