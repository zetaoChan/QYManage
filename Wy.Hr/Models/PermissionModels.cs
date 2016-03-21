using Newtonsoft.Json;

namespace Wy.Hr.Models
{

    public class PermissionPagedArgs : PagedArgs
    {
        public string Name { get; set; }
    }

    public class PermissionModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}