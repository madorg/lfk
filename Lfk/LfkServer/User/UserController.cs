using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Models.User;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkServer.User
{
    class UserController
    {
        public static NetworkOperationInfo HandleRequest(string action, object data)
        {
            NetworkOperationInfo operationInfo = null;

            switch (action)
            {
                case UserNetworkActions.Login:
                    LoginUser loginUser = JsonDeserializer.DeserializeObjectFromFile<LoginUser>(data.ToString());
                    // подключение к БД, необходимые действия, формирование operationInfo
                    break;

                case UserNetworkActions.Signup:
                    SignupUser signupUser = JsonDeserializer.DeserializeObjectFromFile<SignupUser>(data.ToString());
                    // подключение к БД, необходимые действия, формирование opeartionInfo
                    break;

                default:
                    break;
            }

            operationInfo = new NetworkOperationInfo()
            {
                Code = NetworkStatusCodes.Ok,
                Message = "Это сообщение для пользователя"
            };

            return operationInfo;
        }
    }
}