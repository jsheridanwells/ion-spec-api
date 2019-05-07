using Newtonsoft.Json;

namespace LandonApi.Models
{
    public class ResourceBase
    {
        [JsonProperty(Order = -2)]
        public string Href { get; set; }
    }
}
