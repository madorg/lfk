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

namespace LfkServer.Repository
{
    class RepositoryController
    {
        public static ResponseNetworkPackage HandleRequest(string action, object data)
        {
            ServerRepository serverRepository = JsonDeserializer.DeserializeObject<ServerRepository>(data.ToString());

            RepositoryConnector repositoryConnector = new RepositoryConnector();
            NetworkOperationInfo operationInfo = null;

            try
            {
                switch (action)
                {
                    case RepositoryNetworkActions.Create:
                        repositoryConnector.HandleCreate(serverRepository);
                        break;

                    case RepositoryNetworkActions.Read:
                        // подключение к БД, поиск соответсвующей записи, формирование ответа
                        break;

                    case RepositoryNetworkActions.Update:
                        repositoryConnector.HandleUpdate(serverRepository);
                        break;

                    case RepositoryNetworkActions.Delete:
                        // подключение к БД, поиск соответсвующей записи, формирование ответа
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

            ResponseNetworkPackage responsePackage = 
                NetworkPackageController.ConvertBytesToPackage<ResponseNetworkPackage>(
                    NetworkPackageController.ConvertDataToBytes(operationInfo, null));

            return responsePackage;
        }
    }
}