using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Models.User;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;
using LfkServer.Database;
using System.Data.SqlClient;
using System;
using LfkExceptions;
using NLog;

namespace LfkServer.User
{
    class UserController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static ResponseNetworkPackage HandleRequest(string action, object data)
        {
            logger.Debug("Запуск обработки запроса к данным User, действие = " + action);

            ResponseNetworkPackage responsePackage = new ResponseNetworkPackage();
            UserConnector userConnector = new UserConnector();

            Guid guid = Guid.Empty;
            NetworkOperationInfo operationInfo = null;

            try
            {
                switch (action)
                {
                    case UserNetworkActions.Login:
                        LoginUser loginUser = data as LoginUser;
                        guid = userConnector.Read(loginUser);
                        break;

                    case UserNetworkActions.Signup:
                        SignupUser signupUser = data as SignupUser;       
                        guid = userConnector.Create(signupUser);
                        break;

                    default:
                        break;
                }

                operationInfo = new NetworkOperationInfo()
                {
                    Code = NetworkStatusCodes.Ok,
                    Message = "OK"
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
                        NetworkPackageController.ConvertDataToBytes(operationInfo, guid));
                logger.Debug("Пакет с ответом для запроса к данным User (" + action + ") сформирован");
            }

            logger.Debug("Завершение обработки запроса к данным User, действие = " + action);
            return responsePackage;
        }
    }
}