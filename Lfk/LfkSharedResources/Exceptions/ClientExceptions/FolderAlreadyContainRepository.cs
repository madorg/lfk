using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class FolderAlreadyContainRepositoryException : ClientException
    {
        public FolderAlreadyContainRepositoryException(string message) : base(message)
        {
        }
    }
}
