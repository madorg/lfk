using LfkExceptions;
using LfkSharedResources.Extensions;
using LfkSharedResources.Networking;
using LfkSharedResources.Networking.NetworkPackages;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LfkClient.ServerConnection
{
    public class DataSender
    {
        public static ResponseNetworkPackage Send(TcpClient tcpClient, byte[] data)
        {
            ResponseNetworkPackage responsePackage = null;

            try
            {
                tcpClient.Connect(ServerInformation.Hostname, ServerInformation.Port);
                tcpClient.GetStream().Write(data, 0, data.Length);

                byte[] readedData = null;
                while (tcpClient.IsConnected())
                {
                    if (tcpClient.Available != 0)
                    {
                        Thread.Sleep(30);
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