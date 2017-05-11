using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class LfkSharedResourcesException : LfkException
    {
        public LfkSharedResourcesException(string message) : base(message)
        {

        }
    }
}