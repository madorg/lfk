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
        private string path;

        public FileSystemWriter(string p)
        {
            path = p;
        }

        public void CreateFolder(string folderName)
        {
            Directory.CreateDirectory(path + folderName);   
        }

        public void CreateFile(string fileName)
        {
            File.Create(path + fileName);
        }

        public void AppendToFile(string fileName, string data)
        {
            File.AppendText(fileName).Write(data);
        }
    }
}