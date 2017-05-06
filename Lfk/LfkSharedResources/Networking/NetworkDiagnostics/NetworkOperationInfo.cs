using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkSharedResources.Networking.NetworkDiagnostics
{
    public class NetworkOperationInfo
    {
        public NetworkStatusCodes Code { get; set; }
        public string Message { get; set; }
    }
}
