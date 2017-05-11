using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;

namespace LfkServer.Client.Handlers
{
    /// <summary>
    /// Отвечает за ответ клиенту
    /// </summary>
    class ResponseHandler
    {
        public static async Task HandleResponse (TcpClient client, ResponseNetworkPackage responsePackage)
        {
            // ------------------ START LOG ------------------ //
            Console.WriteLine("ResponseHandler (поток " + Environment.CurrentManagedThreadId + "): начал отправку ответа для клиентского потока");
            // ------------------ END LOG ------------------ //

            byte[] responseData = NetworkPackageController.ConvertDataToBytes(responsePackage.OperationInfo, responsePackage.Data);
            await client.GetStream().WriteAsync(responseData, 0, responseData.Length);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("ResponseHandler (поток " + Environment.CurrentManagedThreadId + "): законил отправку ответа для клиентского потока");
            // ------------------ END LOG ------------------ //
        }
    }
}