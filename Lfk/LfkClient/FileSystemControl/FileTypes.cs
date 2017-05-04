using System;

namespace LfkClient.FileSystemControl
{
    [Flags]
    public enum FileTypes
    {
        Client        = 0x01,
        EntireSystem  = 0x02,
        SystemCommits = 0x04,
        SystemObjects = 0x08
    }
}