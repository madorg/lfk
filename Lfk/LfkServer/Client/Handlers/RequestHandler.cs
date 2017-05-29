using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking;
using System.IO;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkPackages;
using NLog;
using System.Net;

namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Получает запросы клиента и перенаправляет их на необходимую обработку
    /// </summary>
    class RequestHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public async Task<RequestNetworkPackage> HandleRequest(TcpClient client)
        {
            logger.Info("Запуск обработки ЗАПРОСА клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());

            byte[] data = new byte[client.Available];
            logger.Debug("Размер данных запроса от " + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString() + " = " + data.Length + " байт");

            await client.GetStream().ReadAsync(data, 0, data.Length);
            logger.Debug("Запрос от " + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString() + " успешно считан");

            RequestNetworkPackage package = NetworkPackageController.ConvertBytesToPackage<RequestNetworkPackage>(data);
            logger.Debug("Запрос от " + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString() + " успешно десериализован в пакет");

            logger.Info("Завершение обработки ЗАПРОСА клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
            return package;
        }
    }
}