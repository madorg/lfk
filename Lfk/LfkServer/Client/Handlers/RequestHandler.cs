using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking;
using System.IO;

namespace LfkServer.Client.Handlers
{
    /*

    Авторизация
         

    */

    /// <summary>
    /// Получает запросы клиента и перенаправляет их на необходимую обработку
    /// </summary>
    class RequestHandler
    {
        public async Task<NetworkPackage> HandleRequest(Stream stream)
        {
            byte[] dataLength = new byte[4];
            stream.Read(dataLength, 0, dataLength.Length);
            int packageSize = BitConverter.ToInt32(dataLength, 0);
            byte[] data = new byte[packageSize];
            stream.Read(data, 0, packageSize);

            NetworkPackageController packageController = new NetworkPackageController();
            NetworkPackage package = packageController.ConvertBytesToPackage(data);

            return package;
        }
    }
}