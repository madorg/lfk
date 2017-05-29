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
using NLog;

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

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static TcpListener serverListener;

        private Server()
        {
            serverListener = new TcpListener(ServerInformation.IP, ServerInformation.Port);
        }

        private void Start()
        {
            serverListener.Start();
            logger.Info("Ожидание клиентов запущено");

            ClientController clientController = new ClientController();

            while (true)
            {
                TcpClient client = serverListener.AcceptTcpClient();
                logger.Info("Принял клиента с ip = {0}", (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString());

                clientController.HandleClient(client);

                logger.Info("Готов принять нового клиента");
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

            logger.Debug("Запуск программы");
            Server server = new Server();
            server.Start();
        }
    }
}