using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Serialization.Json;

namespace LfkClient.ServerConnection.Handlers
{
    class ReadHandler
    {
        public static ResponseNetworkPackage Read(TcpClient tcpClient, byte[] data)
        {
            tcpClient.Connect(ServerInformation.Hostname, ServerInformation.Port);
            tcpClient.GetStream().Write(data, 0, data.Length);
            byte[] readedData = null;
            while (tcpClient.Connected)
            {
                if (tcpClient.Available != 0)
                {
                    readedData = new byte[tcpClient.Available];
                    tcpClient.GetStream().Read(readedData, 0, readedData.Length);
                    break;
                }
            }

            ResponseNetworkPackage responsePackage = NetworkPackageController.ConvertBytesToPackage<ResponseNetworkPackage>(readedData);
            tcpClient.Close();

            return responsePackage;
        }
    }
}