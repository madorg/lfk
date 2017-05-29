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
using NLog;

namespace LfkServer.Repository
{
    class RepositoryController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static ResponseNetworkPackage HandleRequest(string action, object data)
        {
            logger.Debug("Запуск обработки запроса к данным Repository, действие = " + action);

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
                        repositoryConnector.HandleDelete(data.ToString());
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
                    Message = "ОК"
                };
            }
            catch (ServerException serverException)
            {
                logger.Error("Обработка исключения ServerException: " + serverException.Message);
                operationInfo = new NetworkOperationInfo()
                {
                    Code = NetworkStatusCodes.Fail,
                    Message = serverException.Message
                };
            }
            finally
            {
                responsePackage =
                        NetworkPackageController.ConvertBytesToPackage<ResponseNetworkPackage>(
                            NetworkPackageController.ConvertDataToBytes(operationInfo, responseData));
                logger.Debug("Пакет с ответом для запроса к данным Repository (" + action + ") сформирован");
            }

            logger.Debug("Завершение обработки запроса к данным Repository, действие = " + action);
            return responsePackage;
        }
    }
}