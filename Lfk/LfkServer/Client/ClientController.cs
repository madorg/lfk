using System;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client.Handlers;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkServer.Repository;
using LfkServer.User;

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

            NetworkOperationInfo operationInfo = null;
            switch (package.Destination)
            {
                case NetworkPackageDestinations.User:
                    operationInfo = UserController.HandleRequest(package.Action, package.Data);
                    break;

                case NetworkPackageDestinations.Repository:
                    operationInfo = RepositoryController.HandleRequest(package.Action, package.Data);
                    break;

                default:
                    break;
            }

            await ResponseHandler.HandleResponse(client, operationInfo);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("ClientController (поток " + Environment.CurrentManagedThreadId + "): закончил свою работу с клиентским потоком");
            // ------------------ END LOG ------------------ //
        }
    }
}