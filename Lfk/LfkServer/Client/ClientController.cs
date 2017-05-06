using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client.Handlers;
using System.IO;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Models.Repository;
using LfkSharedResources.Serialization.Json;

namespace LfkServer.Client
{
    class ClientController
    {
        private RequestHandler requestHandler;
        private ResponseHandler responseHandler;

        public ClientController()
        {
            requestHandler = new RequestHandler();
            responseHandler = new ResponseHandler();
        }

        public async void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            // ------------------ START LOG ------------------ //
            Console.WriteLine("ClientController (поток " + Environment.CurrentManagedThreadId + "): принял клиентский поток");
            // ------------------ END LOG ------------------ //

            NetworkPackage package = await requestHandler.HandleRequest(client);

            LocalRepository lp = JsonDeserializer.DeserializeObject<LocalRepository>(package.Data.ToString());


            byte[] responseData = NetworkPackageController.ConvertDataToBytes(default(NetworkPackageDestinations), null, new NetworkOperationInfo() {
                Code = NetworkStatusCodes.Ok,
                Message = "Все равно хороший ответ!!!"
            });

            ResponseHandler.HandleResponse(stream, responseData);
        }
    }
}