using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using LfkSharedResources.Networking.NetworkDiagnostics;
using LfkSharedResources.Networking;
namespace LfkClient.ServerConnection.Handlers
{
    class CreateHandler
    {
        public static NetworkOperationInfo Create(TcpClient tcpClient, byte[] data)
        {
            tcpClient.Connect(ServerInformation.Hostname, ServerInformation.Port);
            tcpClient.GetStream().Write(data, 0, data.Length);
            byte[] readedData = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                tcpClient.GetStream().CopyTo(memoryStream);
                readedData = memoryStream.ToArray();
            }
            return NetworkPackageController.ConvertBytesToPackage(readedData).Data as NetworkOperationInfo;
        }
    }
}
