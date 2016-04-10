using Newtonsoft.Json;
using Cn.QYManage.Common;

namespace Cn.QYManage.Models
{

    public class PositionPagedArgs : PagedArgs
    {
        public string Name { get; set; }
    }

    public class PositionModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("no")]
        public string No { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("grade")]
        public PositionGrade Grade { get; set; }
    }

}