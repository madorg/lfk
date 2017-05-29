using System;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client.Handlers;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkServer.Repository;
using LfkServer.User;
using LfkSharedResources.Networking.NetworkPackages;
using NLog;
using System.Net;

namespace LfkServer.Client
{
    class ClientController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private RequestHandler requestHandler;
        private ResponseHandler responseHandler;

        public ClientController()
        {
            requestHandler = new RequestHandler();
            responseHandler = new ResponseHandler();
        }

        public async void HandleClient(TcpClient client)
        {
            logger.Info("Запуск обработки запроса/ответа клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());

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

            logger.Info("Завершение обработки клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
            client.Close();
        }
    }
}