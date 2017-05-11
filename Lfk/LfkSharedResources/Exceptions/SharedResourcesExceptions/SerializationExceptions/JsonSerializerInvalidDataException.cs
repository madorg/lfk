using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    public class JsonSerializerInvalidDataException : JsonSerializerException
    {
        public JsonSerializerInvalidDataException(string message) : base(message)
        {

        }
    }
}