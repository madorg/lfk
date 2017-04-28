﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LfkClient.FileSystemControl;

namespace LfkClient.Serialization.Json
{
    class JsonSerializer
    {
        public static void SerializeObjectToFile<T>(T data, string fileName)
        {
            string newData = SerializeObject(data);
            FileSystem.WriteToFile(fileName, newData);
        }

        public static string SerializeObject(object obj)
        {
            string result = string.Empty;

            result = JsonConvert.SerializeObject(obj, Formatting.Indented);

            return result;
        }
    }
}