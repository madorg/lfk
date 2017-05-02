using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LfkClient.FileSystemControl
{
    /// <summary>
    /// Отвечает за работу над записью информации на жёсткий диск
    /// </summary>
    internal class FileSystemWriter
    {
        public void CreateFolder(string folderName)
        {
            Directory.CreateDirectory(folderName);   
        }

        public void CreateFile(string fileName)
        {
            using (File.Create(fileName))
            {

            }
        }

        public void InitializeFile(string fileName, string data)
        {
            if (!File.Exists(fileName))
            {
                CreateFile(fileName);
                WriteToFile(fileName, data);
            }
        }

        public void AppendToFile(string fileName, string data)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.Write(data);
            }
        }

        public void WriteToFile(string fileName, string data)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.Write(data);
            }
        }
    }
}