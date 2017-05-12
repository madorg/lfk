using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace LfkSharedResources.Extensions
{
    public static class NetworkExtensions
    {
        public static bool IsConnected(this TcpClient tcpClient)
        {
            try
            {
                bool connected = !(tcpClient.Client.Poll(1, SelectMode.SelectRead) && tcpClient.Client.Available == 0);
                return connected;
            }
            catch
            {
                return false;
            }
        }
    }
}