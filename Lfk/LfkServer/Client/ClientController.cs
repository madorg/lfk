﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LfkSharedResources.Networking;
using LfkServer.Client.Handlers;
using System.IO;

namespace LfkServer.Client
{
    class ClientController
    {
        private RequestHandler requestHandler;
        private ResponseHandler responseHandler;

        public ClientController()
        {
            requestHandler = new RequestHandler();
            responseHandler = new ResponseHandler();
        }

        public async void HandleClient(TcpClient client)
        {
            Stream stream = client.GetStream();

            // ------------------ START LOG ------------------ //
            Console.WriteLine("ClientController (поток " + Environment.CurrentManagedThreadId + "): принял клиентский поток");
            // ------------------ END LOG ------------------ //

            NetworkPackage package = await requestHandler.HandleRequest(stream);
        }
    }
}