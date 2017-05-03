using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace LfkClient.ServerConnection
{ 
    /// <summary>
    /// Фасад подключения в базе данных, отвечающий за запросы к серверу
    /// </summary>
    class ServerConnector
    {
        private static TcpClient tcpClient = new TcpClient("localhost", 4300);

        public static void Create(byte[] data)
        {
            tcpClient.Connect("localhost", 4200);
            tcpClient.GetStream().Write(data, 0, data.Length);
        }

        public static void Find(string data)
        {

        }
    }
}