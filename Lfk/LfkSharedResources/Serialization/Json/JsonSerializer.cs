using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LfkExceptions;

namespace LfkSharedResources.Serialization.Json
{
    public class JsonSerializer
    {
        public static Action<string, string> WriteMethod { get; set; }

        public static void SerializeObjectToFile<T>(T data, string fileName)
        {
            string newData = SerializeObject(data);
            WriteMethod(fileName, newData);
        }

        public static string SerializeObject(object obj)
        {
            string result = string.Empty;

            if (obj == null)
            {
                throw new JsonSerializerNullArgumentException("Невозможна сериализация объектов, указывающих на null");
            }

            try
            {
                result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch (JsonException)
            {
                throw new JsonSerializerInvalidDataException("Данные не подлежат сериализации");
            }

            return result;
        }
    }
}