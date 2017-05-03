using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LfkSharedResources.Serialization.Json
{
    class JsonDeserializer
    {
        public static Func<string, string> ReadMethod { get; set; }

        public static T DeserializeObjectFromFile<T>(string fileName)
        {
            // Обработка исключения "делегат не установлен"
            string oldData = ReadMethod(fileName);
            T deserializedOldData = DeserializeObject<T>(oldData);
            return deserializedOldData;
        }

        public static T DeserializeObject<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}