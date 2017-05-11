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
        public static bool TryLogin(AbstractUser abstractUser, out string message)
        {
            bool rc = false;

            byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.User, UserNetworkActions.Login, abstractUser);
            ResponseNetworkPackage responsePackage = ServerConnector.Read(data);

            if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
            {
                rc = true;
            }

            message = responsePackage.OperationInfo.Message;

            return rc;
        }

        public static bool TrySignup(AbstractUser abstractUser, out string message)
        {
            bool rc = false;

            try
            {
                byte[] data = NetworkPackageController.ConvertDataToBytes(NetworkPackageDestinations.User, UserNetworkActions.Signup, abstractUser);
                ResponseNetworkPackage responsePackage = ServerConnector.Create(data);

                if (responsePackage.OperationInfo.Code == NetworkStatusCodes.Ok)
                {
                    rc = true;
                }

                message = responsePackage.OperationInfo.Message;
            }
            catch (JsonSerializerException jse)
            {
                message = "Возникла ошибка при формировании сетевого пакета. Пожалуйста, проверьте правильность введёных данных и попробуйте ещё раз!";
            }

            return rc;
        }
    }
}