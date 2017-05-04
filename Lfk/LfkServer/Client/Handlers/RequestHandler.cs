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

            byte[] dataLength = new byte[4];
            await stream.ReadAsync(dataLength, 0, dataLength.Length);
            int packageSize = BitConverter.ToInt32(dataLength, 0);
            byte[] data = new byte[packageSize];

            await stream.ReadAsync(data, 0, packageSize);

            NetworkPackageController packageController = new NetworkPackageController();
            NetworkPackage package = packageController.ConvertBytesToPackage(data);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("RequestHandler (поток " + Environment.CurrentManagedThreadId + "): закончил обработку клиентского потока");
            // ------------------ END LOG ------------------ //

            return package;
        }
    }
}