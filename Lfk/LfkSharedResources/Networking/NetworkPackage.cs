using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking.NetworkActions;

namespace LfkSharedResources.Networking
{
    public class NetworkPackage
    {
        public NetworkPackageDestinations Destination { get; internal set; }
        public string Action { get; internal set; }
        public object Data { get; internal set; }
    }
}