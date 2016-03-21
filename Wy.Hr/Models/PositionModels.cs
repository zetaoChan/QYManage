using Newtonsoft.Json;
using Wy.Hr.Common;

namespace Wy.Hr.Models
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