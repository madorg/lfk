using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LfkClient.Serialization.Json
{
    class JsonSerializer
    {
        public static string SerializeObject(object obj)
        {
            string result = string.Empty;

            result = JsonConvert.SerializeObject(obj);

            return result;
        }
    }
}