using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LfkClient.FileSystemControl
{
    /// <summary>
    /// Отвечает за чтение информации с жёсткого диска
    /// </summary>
    class FileSystemReader 
    {
        public string[] ReadWorkingDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo
                .GetFiles("*", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(path.Length + 1))
                .Where(m => !m.StartsWith(@"lfk\"))
                .ToArray();
        }

        public string ReadFileContent(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}