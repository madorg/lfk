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
        public static ResponseNetworkPackage Create(byte[] data)
        {
            ResponseNetworkPackage responsePackage = null;

            using (TcpClient tcpClient = new TcpClient())
            {
                tcpClient.ReceiveTimeout = 1000;
                responsePackage = Handlers.CreateHandler.Create(tcpClient, data);
            }
            
            return responsePackage;
        }

        public static ResponseNetworkPackage Read(byte[] data)
        {
            ResponseNetworkPackage responsePackage = null;

            using (TcpClient tcpClient = new TcpClient())
            {
                responsePackage = Handlers.ReadHandler.Read(tcpClient, data);
            }

            return responsePackage;
        }

        public static NetworkOperationInfo Update(byte[] data)
        {
            NetworkOperationInfo operationInfo = null;
            return operationInfo;
        }

        public static NetworkOperationInfo Delete(byte[] data)
        {
            NetworkOperationInfo operationInfo = null;
            return operationInfo;
        }
    }
}