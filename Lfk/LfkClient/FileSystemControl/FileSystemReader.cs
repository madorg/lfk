using System.Linq;
using System.IO;
using System;
using System.Text;

namespace LfkClient.FileSystemControl
{
    /// <summary>
    /// Отвечает за чтение информации с жёсткого диска
    /// </summary>
    class FileSystemReader 
    {
        public string[] ReadWorkingDirectoryFiles(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo
                .GetFiles("*", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(path.Length))
                .Where(m => !m.StartsWith(FileSystemPaths.LfkMainFolder))
                .ToArray();
        }

        public string[] ReadWorkingDirectoryFiles(string path, string folder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path + folder);
            return directoryInfo
                .GetFiles("*", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(path.Length))
                .ToArray();
        }

        public string ReadFileContent(string fileName)
        {
            return File.ReadAllText(fileName, Encoding.Unicode);
        }

        internal bool IsFolderExist(string path)
        {
            return Directory.Exists(path);
        }

        internal Encoding GetFileEncoding(string fileName)
        {
            Encoding e = null;

            using (StreamReader sr = new StreamReader(fileName))
            {
                e = sr.CurrentEncoding;
            }

            return e;
        }
    }
}