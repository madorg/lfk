using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client;
using System.Diagnostics;
using System.IO;

// TODO: убрать консоль-логгирование, обсудить и разработать адекватную файловую систему логгирования

namespace LfkServer
{
    public class Server
    {
        #region Обработка критического завершения программы

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType signal);
        private static EventHandler handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                default:
                    // Освобождение ресурсов 
                    return false;
            }
        }

        #endregion

        private static TcpListener serverListener;

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
            //handler += Handler;
            //SetConsoleCtrlHandler(handler, true);

            Server server = new Server();
            server.Start();
        }
    }
}