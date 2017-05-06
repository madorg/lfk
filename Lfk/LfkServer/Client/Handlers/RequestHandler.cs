using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking;
using System.IO;

namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Получает запросы клиента и перенаправляет их на необходимую обработку
    /// </summary>
    class RequestHandler
    {
        public async Task<NetworkPackage> HandleRequest(Stream stream)
        {
            // ------------------ START LOG ------------------ //
            Console.WriteLine("RequestHandler (поток " + Environment.CurrentManagedThreadId + "): начал обработку клиентского потока");
            // ------------------ END LOG ------------------ //

            byte[] data;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = new byte[memoryStream.Length];
                await memoryStream.ReadAsync(data, 0, data.Length);
            }

            NetworkPackage package = NetworkPackageController.ConvertBytesToPackage(data);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("RequestHandler (поток " + Environment.CurrentManagedThreadId + "): закончил обработку клиентского потока");
            // ------------------ END LOG ------------------ //

            return package;
        }
    }
}