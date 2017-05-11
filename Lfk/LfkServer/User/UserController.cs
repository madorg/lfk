using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Models.User;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking.NetworkPackages;
using LfkServer.Database;
using System.Data.SqlClient;
using System;

namespace LfkServer.User
{
    class UserController
    {
        public static ResponseNetworkPackage HandleRequest(string action, object data)
        {
            ResponseNetworkPackage responsePackage = new ResponseNetworkPackage();

            Guid guid = Guid.Empty;
            NetworkOperationInfo operationInfo = new NetworkOperationInfo();

            switch (action)
            {
                case UserNetworkActions.Login:
                    LoginUser loginUser = JsonDeserializer.DeserializeObject<LoginUser>(data.ToString());
                    break;

                case UserNetworkActions.Signup:
                    SignupUser signupUser = JsonDeserializer.DeserializeObject<SignupUser>(data.ToString());
                    UserConnector userConnector = new UserConnector();
                    guid = userConnector.Create(signupUser);
                    operationInfo.Code = NetworkStatusCodes.Ok;
                    operationInfo.Message = "Вы успешно зарегистрировались!";
                    break;

                default:
                    break;
            }


            // finaly {
            //responsePackage = NetworkPackageController.ConvertDataToBytes(operationInfo, guid);
            // }

            return responsePackage;
        }
    }
}