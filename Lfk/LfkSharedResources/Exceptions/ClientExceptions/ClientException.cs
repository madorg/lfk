using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class ClientException : LfkException
    {
        public ClientException(string message) : base(message)
        {

        }
    }
}