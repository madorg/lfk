using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Отвечает за ответ клиенту
    /// </summary>
    class ResponseHandler
    {
        public static void HandleResponse(Stream stream,byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
    }
}