using System;

namespace LfkClient.FileSystemControl
{
    /// <summary>
    /// Битовая последовательность, представляющая все типы файлов, содержащихся в рабочем каталоге
    /// </summary>
    [Flags]
    public enum FileTypes
    {
        Client        = 0x01,
        EntireSystem  = 0x02,
        SystemCommits = 0x04,
        SystemObjects = 0x08
    }
}