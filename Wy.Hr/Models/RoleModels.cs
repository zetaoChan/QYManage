using Newtonsoft.Json;

namespace Wy.Hr.Models
{

    public class RolePagedArgs : PagedArgs
    {
    }

    public class RoleModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sysName")]
        public string SysName { get; set; }
        [JsonProperty("permissionIds")]
        public string PermissionIds { get; set; }
    }

    public class RoleBatchDelArgs
    {
        public int[] Ids { get; set; }
    }

}