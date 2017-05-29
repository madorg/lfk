using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Serialization.Json;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkSharedResources.Networking
{
    public class NetworkPackageController
    {
        public static byte[] ConvertDataToBytes(NetworkPackageDestinations destination, string action, object data)
        {
            string serializedPackage = string.Empty;

            try
            {
                NetworkPackage package = CreateRequestPackage(destination, action, data);
                serializedPackage = JsonSerializer.SerializeObject(package);
            }
            catch
            {
                throw;
            }

            return Encoding.UTF8.GetBytes(serializedPackage);
        }

        public static T ConvertDataToPackage<T>(NetworkOperationInfo operationInfo, object data)
        {
            byte[] byteData = ConvertDataToBytes(operationInfo, data);
            T package = ConvertBytesToPackage<T>(byteData);
            return package;
        }

        public static byte[] ConvertDataToBytes(NetworkOperationInfo operationInfo, object data)
        {
            NetworkPackage package = CreateResponsePackage(operationInfo, data);
            string serializedPackage = JsonSerializer.SerializeObject(package);
            return Encoding.UTF8.GetBytes(serializedPackage);
        }

        public static T ConvertBytesToPackage<T>(byte[] data)
        {
            string serializedPackage = Encoding.UTF8.GetString(data);
            T package = JsonDeserializer.DeserializeObject<T>(serializedPackage);
            return package;
        }

        private static NetworkPackage CreateRequestPackage(NetworkPackageDestinations destination, string action, object data)
        {
            NetworkPackage package = new RequestNetworkPackage()
            {
                Destination = destination,
                Action = action,
                Data = data
            };

            return package;
        }

        private static NetworkPackage CreateResponsePackage(NetworkOperationInfo operationInfo, object data)
        {
            NetworkPackage package = new ResponseNetworkPackage()
            {
                OperationInfo = operationInfo,
                Data = data
            };

            return package;
        }

        public static void SetOperationInfo(ResponseNetworkPackage package, NetworkStatusCodes statusCode, string message)
        {
            NetworkOperationInfo operationInfo = new NetworkOperationInfo()
            {
                Code = statusCode,
                Message = message
            };

            package.OperationInfo = operationInfo;
        }
    }
}