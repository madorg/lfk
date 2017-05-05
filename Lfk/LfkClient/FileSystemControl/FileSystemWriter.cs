using System.IO;

namespace LfkClient.FileSystemControl
{
    /// <summary>
    /// Отвечает за работу над записью информации на жёсткий диск
    /// </summary>
    internal class FileSystemWriter
    {
        public void CreateFolder(string folderName)
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(folderName);

            if (folderName.EndsWith(FileSystemPaths.LfkMainFolder))
            {
                directoryInfo.Attributes = FileAttributes.Hidden;
            }
        }

        public void CreateFile(string fileName)
        {
            using (File.Create(fileName)) { }
        }

        public void InitializeFile(string fileName, string data)
        {
            if (!File.Exists(fileName))
            {
                CreateFile(fileName);
                WriteToFile(fileName, data);
            }
        }

        public void AppendToFile(string fileName, string data)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.Write(data);
            }
        }

        public void WriteToFile(string fileName, string data)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.Write(data);
            }
        }

        public void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}