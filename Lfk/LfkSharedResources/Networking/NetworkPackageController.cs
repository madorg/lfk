using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking.NetworkActions;
using LfkSharedResources.Serialization.Json;

namespace LfkSharedResources.Networking
{
    public class NetworkPackageController
    {
        public byte[] ConvertDataToBytes(NetworkPackageDestinations destination, string action, object data)
        {
            NetworkPackage package = CreatePackage(destination, action, data);
            string serializedPackage = JsonSerializer.SerializeObject(package);
            return Encoding.UTF8.GetBytes(serializedPackage);
        }

        public NetworkPackage ConvertBytesToPackage(byte[] data)
        {
            string serializedPackage = Encoding.UTF8.GetString(data);
            NetworkPackage package = JsonDeserializer.DeserializeObject<NetworkPackage>(serializedPackage);
            return package;
        }

        private NetworkPackage CreatePackage(NetworkPackageDestinations destination, string action, object data)
        {
            NetworkPackage package = new NetworkPackage()
            {
                Destination = destination,
                Action = action,
                Data = data
            };

            return package;
        }
    }
}