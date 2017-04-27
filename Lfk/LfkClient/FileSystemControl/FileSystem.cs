using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.FileSystemControl
{
    public static class FileSystem
    {
        private static FileSystemWriter writer;
        private static FileSystemReader reader;

        public static string Path { get; set; }

        static FileSystem()
        {
            writer = new FileSystemWriter();
            reader = new FileSystemReader();
        }

        // --- Writing ---

        public static void CreateFolder(string folderName)
        {
            writer.CreateFolder(Path + folderName);
        }

        public static void CreateFile(string fileName)
        {
            writer.CreateFile(Path + fileName);
        }

        public static void AppendToFile(string fileName, string data)
        {
            writer.AppendToFile(Path + fileName, data);
        }

        public static void WriteToFile(string fileName, string data)
        {
            writer.WriteToFile(Path + fileName, data);
        }

        // --- Reading ---

        public static string[] ReadWorkingDirectory(/* enum ТИП_СЧИТЫВАЕМЫХ_ФАЙЛОВ */)
        {
            return reader.ReadWorkingDirectory(Path);
        }

        public static string ReadFileContent(string fileName)
        {
            return reader.ReadFileContent(Path + fileName);
        }
    }
}