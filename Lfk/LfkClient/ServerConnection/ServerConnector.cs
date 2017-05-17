using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkPackages;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkClient.ServerConnection
{ 
    /// <summary>
    /// Фасад подключения к серверу
    /// </summary>
    class ServerConnector
    {
        public static ResponseNetworkPackage Send(byte[] data)
        {
            ResponseNetworkPackage responsePackage = null;

            using (TcpClient tcpClient = new TcpClient())
            {
                responsePackage = DataSender.Send(tcpClient, data);
            }

            return responsePackage;
        }
    }
}