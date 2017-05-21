using Newtonsoft.Json;

namespace LfkSharedResources.Networking.NetworkPackages
{
    [JsonObject]
    public abstract class NetworkPackage
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public object Data { get; internal set; }
    }
}