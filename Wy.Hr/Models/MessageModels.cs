using System;
using Newtonsoft.Json;

namespace Wy.Hr.Models
{

    public class MessagePagedArgs : PagedArgs
    {
        public bool? IsReaded { get; set; }
    }

    public class MessageModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("senderName")]
        public string SenderName { get; set; }
        [JsonProperty("recipientName")]
        public string RecipientName { get; set; }
        [JsonProperty("isReaded")]
        public bool IsReaded { get; set; }
        [JsonProperty("sendDate")]
        public DateTime SendDate { get; set; }
        [JsonProperty("contents")]
        public string Contents { get; set; }
    }

}