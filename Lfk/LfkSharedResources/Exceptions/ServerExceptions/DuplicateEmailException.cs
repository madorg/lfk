using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class DuplicateEmailException : ServerException
    {
        public DuplicateEmailException(string message) : base(message)
        {

        }
    }
}