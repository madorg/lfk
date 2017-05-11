using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class ServerConnectionException : ClientException
    {
        public ServerConnectionException(string message) : base(message)
        {
                
        }
    }
}