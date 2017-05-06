using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkClient.ServerConnection
{ 
    /// <summary>
    /// Фасад подключения к серверу
    /// </summary>
    class ServerConnector
    {
        private static TcpClient tcpClient = new TcpClient();

        public static NetworkOperationInfo Create(byte[] data)
        {
            NetworkOperationInfo operationInfo = null;
            operationInfo = Handlers.CreateHandler.Create(tcpClient, data);
            return operationInfo;
        }

        public static NetworkOperationInfo Read(byte[] data)
        {
            NetworkOperationInfo operationInfo = null;
            return operationInfo;
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