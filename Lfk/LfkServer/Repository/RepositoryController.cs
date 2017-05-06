using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Models.Repository;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkServer.Repository
{
    class RepositoryController
    {
        public static NetworkOperationInfo HandleRequest(string action, object data)
        {
            LocalRepository localRepository = JsonDeserializer.DeserializeObject<LocalRepository>(data.ToString());

            switch (action)
            {
                case RepositoryNetworkActions.Create:
                    // подключение к БД, поиск соответсвующей записи, формирование ответа
                    break;

                case RepositoryNetworkActions.Read:
                    // подключение к БД, поиск соответсвующей записи, формирование ответа
                    break;

                case RepositoryNetworkActions.Update:
                    // подключение к БД, поиск соответсвующей записи, формирование ответа
                    break;

                case RepositoryNetworkActions.Delete:
                    // подключение к БД, поиск соответсвующей записи, формирование ответа
                    break;

                default:
                    break;
            }

            NetworkOperationInfo operationInfo = new NetworkOperationInfo()
            {
                Code = NetworkStatusCodes.Ok,
                Message = "Это сообщение для пользователя"
            };

            return operationInfo;
        }
    }
}