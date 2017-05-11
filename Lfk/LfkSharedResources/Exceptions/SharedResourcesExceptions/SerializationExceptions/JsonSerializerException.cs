using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class JsonSerializerException : LfkSharedResourcesException
    {
        public JsonSerializerException(string message) : base(message)
        {

        }
    }
}