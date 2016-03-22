using System;
using Newtonsoft.Json;
using Wy.Hr.Common;

namespace Wy.Hr.Models
{

    public class TaskPagedArgs : PagedArgs
    {
    }

    public class TaskModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("contents")]
        public string Contents { get; set; }
        [JsonProperty("status")]
        public TaskStatus Status { get; set; }
        [JsonProperty("addTime")]
        public DateTime AddTime { get; set; }
        [JsonProperty("addUser")]
        public string AddUser { get; set; }
        [JsonProperty("executor")]
        public string Executor { get; set; }
        [JsonProperty("finishedTime")]
        public DateTime? FinishedTime { get; set; }
        [JsonProperty("expectedTime")]
        public DateTime ExpectedTime { get; set; }
    }

}