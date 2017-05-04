using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking;

namespace LfkServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpl = new TcpListener(IPAddress.Parse("127.0.0.1"), 4200);
            Console.WriteLine(tcpl.LocalEndpoint.AddressFamily.ToString());

            tcpl.Start();

            while (true)
            {
                TcpClient client = tcpl.AcceptTcpClient();
                Console.WriteLine("Is connected = " + client.Connected);
       
                byte[] dataLength = new byte[4];
                client.GetStream().Read(dataLength, 0, dataLength.Length);
                int packageSize = BitConverter.ToInt32(dataLength, 0);
                byte[] data = new byte[packageSize];
                client.GetStream().Read(data, 0, packageSize);

                NetworkPackageController packageController = new NetworkPackageController();
                NetworkPackage package = packageController.ConvertBytesToPackage(data);

                //Console.WriteLine("Client data: " + package.Action);

                

                //byte[] data = client.GetStream().re
            }
        }
    }
}
