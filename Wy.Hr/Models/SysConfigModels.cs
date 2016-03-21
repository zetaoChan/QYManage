using Newtonsoft.Json;

namespace Wy.Hr.Models
{

    public class SysConfigModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sysName")]
        public string SysName { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

}