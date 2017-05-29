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
            Encoding fileEncoding = GetFileEncoding(fileName);
            string fileContent = File.ReadAllText(fileName, Encoding.Default);

            if (fileEncoding.BodyName != Encoding.Unicode.BodyName)
            {
                byte[] oldBytes = fileEncoding.GetBytes(fileContent);
                byte[] newBytes = Encoding.Convert(fileEncoding, Encoding.Unicode, oldBytes);
                fileContent = Encoding.Unicode.GetString(newBytes);
                File.WriteAllText(fileName, fileContent, Encoding.Unicode);
            }

            return fileContent;
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
                sr.Peek();
                e = sr.CurrentEncoding;
            }

            return e;
        }
    }
}