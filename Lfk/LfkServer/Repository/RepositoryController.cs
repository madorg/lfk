using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Models.Repository;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;
using System.Data.SqlClient;
using System;
using LfkServer.Database;
using LfkSharedResources.Networking;
using LfkExceptions;
using System.Collections.Generic;

namespace LfkServer.Repository
{
    class RepositoryController
    {
        public static ResponseNetworkPackage HandleRequest(string action, object data)
        {
            ServerRepository serverRepository = null;
            RepositoryConnector repositoryConnector = new RepositoryConnector();

            NetworkOperationInfo operationInfo = null;
            ResponseNetworkPackage responsePackage = null;
            object responseData = null;

            try
            {
                switch (action)
                {
                    case RepositoryNetworkActions.Create:
                        serverRepository = data as ServerRepository;
                        repositoryConnector.HandleCreate(serverRepository);
                        break;

                    case RepositoryNetworkActions.Read:
                        serverRepository = repositoryConnector.HandleRead(data.ToString());
                        responseData = serverRepository;
                        break;

                    case RepositoryNetworkActions.Update:
                        serverRepository = data as ServerRepository;
                        repositoryConnector.HandleUpdate(serverRepository);
                        break;

                    case RepositoryNetworkActions.Delete:
                        // подключение к БД, поиск соответсвующей записи, формирование ответа
                        break;

                    case RepositoryNetworkActions.View:
                        List<LocalRepository> repositories = repositoryConnector.HandleView(data.ToString());
                        responseData = repositories;
                        break;

                    default:
                        break;
                }

                operationInfo = new NetworkOperationInfo()
                {
                    Code = NetworkStatusCodes.Ok,
                    Message = "Это сообщение для пользователя"
                };
            }
            catch (ServerException serverException)
            {
                operationInfo = new NetworkOperationInfo()
                {
                    Code = NetworkStatusCodes.Fail,
                    Message = serverException.Message
                };
            }

            responsePackage =
                    NetworkPackageController.ConvertBytesToPackage<ResponseNetworkPackage>(
                        NetworkPackageController.ConvertDataToBytes(operationInfo, responseData));

            return responsePackage;
        }
    }
}