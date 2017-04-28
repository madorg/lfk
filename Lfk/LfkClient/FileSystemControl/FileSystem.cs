using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkClient.FileSystemControl
{
    public enum FilesType
    {
        Client        = 0x01,
        EntireSystem  = 0x02,
        SystemCommits = 0x04,
        SystemObjects = 0x08
    }
    
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

        public static string[] ReadWorkingDirectory(FilesType filesType)
        {
            switch (filesType)
            {
                case FilesType.Client:
                    return reader.ReadWorkingDirectory(Path);
                case FilesType.EntireSystem:
                    return reader.ReadWorkingDirectory(Path, @"\lfk\");
                case FilesType.SystemCommits:
                    return reader.ReadWorkingDirectory(Path, "\\lfk\\commits\\");
                case FilesType.SystemObjects:
                    return reader.ReadWorkingDirectory(Path, @"\lfk\objects\");
                default:
                    return null;
            }
        }

        public static string ReadFileContent(string fileName)
        {
            return reader.ReadFileContent(Path + fileName);
        }
    }
}