namespace LfkExceptions
{
    public class DuplicateRepositoryTitleException : ServerException
    {
        public DuplicateRepositoryTitleException(string message) : base(message)
        {

        }
    }
}