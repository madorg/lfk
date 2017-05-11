using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking;
using System.IO;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkPackages;

namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Получает запросы клиента и перенаправляет их на необходимую обработку
    /// </summary>
    class RequestHandler
    {
        public async Task<RequestNetworkPackage> HandleRequest(TcpClient client)
        {
            // ------------------ START LOG ------------------ //
            Console.WriteLine("RequestHandler (поток " + Environment.CurrentManagedThreadId + "): начал обработку клиентского потока");
            // ------------------ END LOG ------------------ //

            byte[] data = new byte[client.Available];
            await client.GetStream().ReadAsync(data, 0, data.Length);

            RequestNetworkPackage package = NetworkPackageController.ConvertBytesToPackage<RequestNetworkPackage>(data);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("RequestHandler (поток " + Environment.CurrentManagedThreadId + "): закончил обработку клиентского потока");
            // ------------------ END LOG ------------------ //

            return package;
        }
    }
}