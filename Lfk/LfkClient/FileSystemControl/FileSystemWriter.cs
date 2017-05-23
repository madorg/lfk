using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

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
            using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.Unicode))
            {
                sw.Write(data);
            }
        }

        public void WriteToFile(string fileName, string data)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.Unicode))
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

        public void CreateFileWithFolders(string filename)
        {
            try
            {
                CreateFile(filename);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                List<string> paths = new List<string>(filename.Split('\\'));
                paths.RemoveAt(paths.Count - 1);

                string fullPath = string.Empty;
                foreach (string folder in paths)
                {
                    fullPath += folder + '\\';
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                }

                CreateFile(filename);
            }
        }

        internal void DeleteFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

        }
    }
}