using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;
using System.Net;
using NLog;

namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Отвечает за ответ клиенту
    /// </summary>
    class ResponseHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static async Task HandleResponse (TcpClient client, ResponseNetworkPackage responsePackage)
        {
            logger.Info("Запуск формирования ОТВЕТА клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());

            byte[] responseData = NetworkPackageController.ConvertDataToBytes(responsePackage.OperationInfo, responsePackage.Data);
            logger.Debug("Размер данных ответа для " + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString() + " = " + responseData.Length + " байт");

            await client.GetStream().WriteAsync(responseData, 0, responseData.Length);
            logger.Debug("Ответ для " + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString() + " успешно записан в клиентский поток");

            logger.Info("Завершение формирования ОТВЕТА клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
        }
    }
}