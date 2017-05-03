using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace LfkServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpl = new TcpListener(IPAddress.Parse("127.0.0.1"), 4200);

            tcpl.Start();

            while (true)
            {
                TcpClient client = tcpl.AcceptTcpClient();


            }
        }
    }
}
