using System;
using Newtonsoft.Json;

namespace Cn.QYManage.Models
{

    public class MessagePagedArgs : PagedArgs
    {
        public bool? IsReaded { get; set; }
        public string Type { get; set; }
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

    public class MessageAddModel
    {
        public int[] RecipientIds { get; set; }
        public string Contents { get; set; }
    }

}