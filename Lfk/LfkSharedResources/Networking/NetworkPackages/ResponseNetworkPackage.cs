using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LfkSharedResources.Networking.NetworkDiagnostics;

namespace LfkSharedResources.Networking.NetworkPackages
{
    [JsonObject]
    public class ResponseNetworkPackage : NetworkPackage
    {
        [JsonProperty]
        public NetworkOperationInfo OperationInfo { get; internal set; }
        [JsonProperty]
        public object Data { get; internal set; }

        public ResponseNetworkPackage()
        {
            OperationInfo = new NetworkOperationInfo();
        }
    }
}