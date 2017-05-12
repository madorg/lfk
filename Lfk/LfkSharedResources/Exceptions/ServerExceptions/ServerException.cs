using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class ServerException : LfkException
    {
        public ServerException(string message) : base(message)
        {

        }
    }
}