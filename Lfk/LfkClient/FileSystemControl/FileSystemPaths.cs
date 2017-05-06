namespace LfkClient.FileSystemControl
{
    /// <summary>
    /// Хранит информацию о путях, относительно которых работает класс FileSystem
    /// </summary>
    static class FileSystemPaths
    {
        public const string LfkMainFolder = @"\.lfk\";

        public const string LfkCommitsFolder = @"\.lfk\commits\";
        public const string LfkObjectsFolder = @"\.lfk\objects\";

        public const string LfkIncludedFile = @"\.lfk\included.json";
        public const string LfkIndexFile    = @"\.lfk\index.json";
        public const string LfkInfoFile     = @"\.lfk\info.json";
    }
}