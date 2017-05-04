using System.Linq;
using System.IO;

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
            return File.ReadAllText(fileName);
        }
    }
}