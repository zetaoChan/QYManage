using System;
using Newtonsoft.Json;

namespace Wy.Hr.Models
{

    public class UserPagedArgs : PagedArgs
    {
    }

    public class UserModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("roleIds")]
        public string RoleIds { get; set; }
    }

    public class UserAddModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleIds { get; set; }
    }

    public class UserEditModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleIds { get; set; }
    }

    public class UserBatchDelArgs
    {
        public int[] Ids { get; set; }
    }

    public class UserChangePswArgs
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AutoComplateArgs
    {
        public string Query { get; set; }
    }

}