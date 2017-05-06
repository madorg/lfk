using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace LfkSharedResources.Networking.NetworkDiagnostics
{
    [JsonObject]
    public class NetworkOperationInfo
    {
        [JsonProperty]
        public NetworkStatusCodes Code { get; set; }
        [JsonProperty]
        public string Message { get; set; }
    }
}
