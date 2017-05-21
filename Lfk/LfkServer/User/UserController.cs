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

namespace LfkServer.User
{
    class UserController
    {
        public static ResponseNetworkPackage HandleRequest(string action, object data)
        {
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
            }

            return responsePackage;
        }
    }
}