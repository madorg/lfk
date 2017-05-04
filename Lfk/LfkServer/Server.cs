using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client;

namespace LfkServer
{
    public class Server
    {
        private TcpListener serverListener;

        private Server()
        {
            serverListener = new TcpListener(ServerInformation.IP, ServerInformation.Port);
        }

        private async void Start()
        {
            serverListener.Start();

            ClientController clientController = new ClientController();

            while (true)
            {
                TcpClient client = serverListener.AcceptTcpClient();
                await clientController.HandleClient(client);

                Console.WriteLine("Something...");
            }
        }

        private void Stop()
        {
            serverListener.Stop();
        }

        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
        }
    }
}