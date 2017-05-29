using System.IO;
using System.Text;

namespace LfkClient.FileSystemControl
{    
    /// <summary>
    /// Отвечает за работу с файловой системой (рабочим каталогом пользователя)
    /// </summary>
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

        #region Методы записи

        /// <summary>
        /// Создаёт новый каталог (если его ещё не существует) в соответствии с указанным путём
        /// </summary>
        /// <param name="folderName">Относительный путь и имя создаваемого каталога</param>
        public static void CreateFolder(string folderName)
        {
            writer.CreateFolder(Path + folderName);
        }

        /// <summary>
        /// Создаёт или перезаписывает файл с указанным именем
        /// </summary>
        /// <param name="fileName">Относительный путь и имя создаваемого файла</param>
        public static void CreateFile(string fileName)
        {
            writer.CreateFile(Path + fileName);
        }

        /// <summary>
        /// Создаёт файл с указанным именем, причём только в том случае, 
        /// если он не существует и записывает в него указанные данные
        /// </summary>
        /// <param name="fileName">Относительный путь и имя создаваемого файла</param>
        /// <param name="data">Данные для записи</param>
        public static void InitializeInexistentFile(string fileName, string data)
        {
            writer.InitializeFile(Path + fileName, data);
        }

        /// <summary>
        /// Добавляет в файл с указанным именем указанные данные
        /// </summary>
        /// <param name="fileName">Относительный путь и имя обновлемого файла</param>
        /// <param name="data">Данные для записи</param>
        public static void AppendToFile(string fileName, string data)
        {
            writer.AppendToFile(Path + fileName, data);
        }

        /// <summary>
        /// Перезаписывает файл с указанным именем указанными данными
        /// </summary>
        /// <param name="fileName">Относительный путь и имя обновлемого файла</param>
        /// <param name="data">Данные для записи</param>
        public static void WriteToFile(string fileName, string data)
        {
            writer.WriteToFile(Path + fileName, data);
        }

        public static void CreateFileWithFolders(string filename)
        {
            writer.CreateFileWithFolders(Path + filename);
        }

        #endregion

        #region Методы чтения

        /// <summary>
        /// Считывает все файлы указанного типа, содержащиеся в рабочей директории
        /// </summary>
        /// <param name="filesType">Битовая последовательность, представляющая типы файлов в рабочем каталоге</param>
        /// <returns>Считанные файлы указанного типа</returns>
        public static string[] ReadWorkingDirectoryFiles(FileTypes filesType)
        {
            switch (filesType)
            {
                case FileTypes.Client:
                    return reader.ReadWorkingDirectoryFiles(Path);
                case FileTypes.EntireSystem:
                    return reader.ReadWorkingDirectoryFiles(Path, FileSystemPaths.LfkMainFolder);
                case FileTypes.SystemCommits:
                    return reader.ReadWorkingDirectoryFiles(Path, FileSystemPaths.LfkCommitsFolder);
                case FileTypes.SystemObjects:
                    return reader.ReadWorkingDirectoryFiles(Path, FileSystemPaths.LfkObjectsFolder);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Считывает содержимое указанного файла
        /// </summary>
        /// <param name="fileName">Относительный путь и имя считываемого файла</param>
        /// <returns>Содержимое указанного файла</returns>
        public static string ReadFileContent(string fileName)
        {
            return reader.ReadFileContent(Path + fileName);
        }

        public static bool IsFolderExist(string path)
        {
            return reader.IsFolderExist(Path + path);
        }

        public static bool IsFileExist(string filename)
        {
            return reader.IsFileExist(Path + filename);
        }

        public static Encoding GetFileEncoding(string fileName)
        {
            return reader.GetFileEncoding(Path + fileName);
        }

        #endregion

        #region Методы удаления / Нужно обсудить еще

        public static void DeleteFile(string fileName)
        {
            writer.DeleteFile(Path + fileName);
        }

        public static void DeleteFolder(string folder)
        {
            writer.DeleteFolder(Path + folder);
        }

        public static void ClearWorkingDirectory()
        {
            foreach (string path in reader.ReadWorkingDirectoryFiles(Path))
            {
                FileAttributes pathType = File.GetAttributes(Path + path);
                if (pathType.HasFlag(FileAttributes.Directory)){
                    writer.DeleteFolder(Path + path);
                }
                else {
                    writer.DeleteFile(Path + path);
                }
            }
        }

        #endregion
    }
}