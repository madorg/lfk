using System.IO;

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

        public static void CreateFile(string fileName, bool rewriteIfExists)
        {
            writer.CreateFile(Path + fileName);
        }

        public static void InitializeInexistentFile(string fileName, string data)
        {
            writer.InitializeFile(Path + fileName, data);
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

        public static string[] ReadWorkingDirectory(FileTypes filesType)
        {
            switch (filesType)
            {
                case FileTypes.Client:
                    return reader.ReadWorkingDirectory(Path);
                case FileTypes.EntireSystem:
                    return reader.ReadWorkingDirectory(Path, FileSystemPaths.LfkMainFolder);
                case FileTypes.SystemCommits:
                    return reader.ReadWorkingDirectory(Path, FileSystemPaths.LfkCommitsFolder);
                case FileTypes.SystemObjects:
                    return reader.ReadWorkingDirectory(Path, FileSystemPaths.LfkObjectsFolder);
                default:
                    return null;
            }
        }

        public static string ReadFileContent(string fileName)
        {
            return reader.ReadFileContent(Path + fileName);
        }

        public static bool FileExists(string fileName)
        {
            return reader.FileExists(Path + fileName);
        }
    }
}