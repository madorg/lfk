using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkExceptions
{
    [Serializable]
    public class LfkException : Exception
    {
        public LfkException(string message) : base(message)
        {

        }
    }
}