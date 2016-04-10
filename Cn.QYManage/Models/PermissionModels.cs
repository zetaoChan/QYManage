using Newtonsoft.Json;

namespace Cn.QYManage.Models
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