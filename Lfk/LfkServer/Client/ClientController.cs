using System;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client.Handlers;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkServer.Repository;
using LfkServer.User;
using LfkSharedResources.Networking.NetworkPackages;

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
            // ------------------ START LOG ------------------ //
            Console.WriteLine("ClientController (поток " + Environment.CurrentManagedThreadId + "): принял клиентский поток");
            // ------------------ END LOG ------------------ //

            RequestNetworkPackage requestPackage = await requestHandler.HandleRequest(client);

            ResponseNetworkPackage responsePackage = null;
            switch (requestPackage.Destination)
            {
                case NetworkPackageDestinations.User:
                    responsePackage = UserController.HandleRequest(requestPackage.Action, requestPackage.Data);
                    break;

                case NetworkPackageDestinations.Repository:
                    responsePackage = RepositoryController.HandleRequest(requestPackage.Action, requestPackage.Data);
                    break;

                default:
                    break;
            }

            await ResponseHandler.HandleResponse(client, responsePackage);

            // ------------------ START LOG ------------------ //
            Console.WriteLine("ClientController (поток " + Environment.CurrentManagedThreadId + "): закончил свою работу с клиентским потоком");
            // ------------------ END LOG ------------------ //
        }
    }
}