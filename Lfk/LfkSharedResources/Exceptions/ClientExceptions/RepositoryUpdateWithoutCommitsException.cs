using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class RepositoryUpdateWithoutCommitsException : ClientException
    {
        public RepositoryUpdateWithoutCommitsException(string message) : base(message)
        {
        }
    }
}
