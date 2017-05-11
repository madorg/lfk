using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Serialization.Json;
using LfkExceptions;

namespace LfkClient.ServerConnection.Handlers
{
    class CreateHandler
    {
        public static ResponseNetworkPackage Create(TcpClient tcpClient, byte[] data)
        {
            ResponseNetworkPackage responsePackage = null;

            try
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

                responsePackage = NetworkPackageController.ConvertBytesToPackage<ResponseNetworkPackage>(readedData);
                tcpClient.Close();
            }
            catch (SocketException)
            {
                throw new ServerConnectionException("Возникла проблема при подключении к серверу. Пожалуйста, повторите запрос позже.");
            }

            return responsePackage;
        }
    }
}
