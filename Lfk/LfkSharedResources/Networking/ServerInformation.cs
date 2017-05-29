using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LfkSharedResources.Networking
{
    public static class ServerInformation
    {
        public static readonly int Port = 4200;
        public static readonly IPAddress IP = IPAddress.Parse("192.168.100.4");
        public static readonly string Hostname = "alexey";
    }
}