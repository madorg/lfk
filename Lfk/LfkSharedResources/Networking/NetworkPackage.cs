using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Networking.NetworkActions;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;

namespace LfkSharedResources.Networking
{
    [JsonObject]
    public class NetworkPackage
    {
        [JsonProperty]
        public NetworkPackageDestinations Destination { get; internal set; }
        [JsonProperty]
        public string Action { get; internal set; }
        [JsonProperty]
        public object Data { get; internal set; }
    }
}