using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client;

// TODO: убрать консоль-логгирование, обсудить и разработать адекватную файловую систему логгирования

namespace LfkServer
{
    public class Server
    {
        private TcpListener serverListener;

        private Server()
        {
            serverListener = new TcpListener(ServerInformation.IP, ServerInformation.Port);
        }

        private void Start()
        {
            serverListener.Start();

            // ------------------ START LOG ------------------ //
            Console.WriteLine("Server (поток " + Environment.CurrentManagedThreadId + "): сервер запущен");
            // ------------------ END LOG ------------------ //

            ClientController clientController = new ClientController();

            // ------------------ START LOG ------------------ //
            Console.WriteLine("Server (поток " + Environment.CurrentManagedThreadId + "): ожидание клиентов");
            // ------------------ END LOG ------------------ //

            while (true)
            {
                TcpClient client = serverListener.AcceptTcpClient();

                // ------------------ START LOG ------------------ //
                Console.WriteLine("Server (поток " + Environment.CurrentManagedThreadId + "): принял клиента с ip = " + (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
                // ------------------ END LOG ------------------ //

                clientController.HandleClient(client);

                // ------------------ START LOG ------------------ //
                Console.WriteLine("Server (поток " + Environment.CurrentManagedThreadId + "): сервер готов принять нового клиента");
                // ------------------ END LOG ------------------ //
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