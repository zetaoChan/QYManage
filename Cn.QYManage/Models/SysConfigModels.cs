using Newtonsoft.Json;

namespace Cn.QYManage.Models
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