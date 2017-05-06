using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Отвечает за ответ клиенту
    /// </summary>
    class ResponseHandler
    {
        public static async Task HandleResponse (TcpClient client, NetworkOperationInfo operationInfo)
        {
            // ------------------ START LOG ------------------ //
            Console.WriteLine("ResponseHandler (поток " + Environment.CurrentManagedThreadId + "): начал отправку ответа для клиентского потока");
            // ------------------ END LOG ------------------ //

            byte[] responseData = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.None, null, operationInfo);
            await client.GetStream().WriteAsync(responseData, 0, responseData.Length);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("ResponseHandler (поток " + Environment.CurrentManagedThreadId + "): законил отправку ответа для клиентского потока");
            // ------------------ END LOG ------------------ //
        }
    }
}