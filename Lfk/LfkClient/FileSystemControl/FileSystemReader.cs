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
                .Select(f => f.FullName.Substring(path.Length))
                .Where(m => !m.StartsWith(@"\lfk\"))
                .ToArray();
        }

        public string[] ReadWorkingDirectory(string path, string folder)
        {
            //p = F:\lfk_test    11
            //folder = \lfk\commits 13
            //d = F:\lfk_test\lfk\commits 

            DirectoryInfo directoryInfo = new DirectoryInfo(path + folder);
            return directoryInfo
                .GetFiles("*", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(path.Length))
                .ToArray();
        }

        public string ReadFileContent(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}