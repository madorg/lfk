using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Models.User;
using LfkSharedResources.Serialization.Json;
using LfkClient.ServerConnection;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkExceptions;

namespace LfkClient.Authorization
{
    /// <summary>
    /// Фасад авторизации пользователя, перенаправляющий запросы пользователя в соответствующие обработчики
    /// </summary>
    public class Authorizator
    {
        public static bool TryLogin(AbstractUser abstractUser, out string message, out Guid userId)
        {
            bool rc = false;
            userId = Guid.Empty;

            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.User, UserNetworkActions.Login, abstractUser);
            ResponseNetworkPackage responsePackage = ServerConnector.Send(data);

            if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {
                rc = true;
                userId = Guid.Parse(responsePackage.Data.ToString());
            }

            message = responsePackage.OperationInfo.Message;

            return rc;
        }

        public static bool TrySignup(AbstractUser abstractUser, out string message, out Guid userId)
        {
            bool rc = false;
            userId = Guid.Empty;

            try
            {
                byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.User, UserNetworkActions.Signup, abstractUser);
                ResponseNetworkPackage responsePackage = ServerConnector.Send(data);

                if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
                {
                    rc = true;
                    userId = Guid.Parse(responsePackage.Data.ToString());
                }

                message = responsePackage.OperationInfo.Message;
            }
            catch (JsonSerializerException)
            {
                message = "Возникла ошибка при формировании сетевого пакета. Пожалуйста, проверьте правильность введёных данных и попробуйте ещё раз!";
            }

            return rc;
        }
    }
}