using System;
using Newtonsoft.Json;
using Wy.Hr.Common;

namespace Wy.Hr.Models
{

    public class MessagePagedArgs : PagedArgs
    {
        public MessageType? MessageType { get; set; }
        public bool? IsReaded { get; set; }
    }

    public class MessageModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("senderId")]
        public int SenderId { get; set; }
        [JsonProperty("senderName")]
        public string SenderName { get; set; }
        [JsonProperty("recipientId")]
        public int RecipientId { get; set; }
        [JsonProperty("recipientName")]
        public string RecipientName { get; set; }
        [JsonProperty("isReaded")]
        public bool IsReaded { get; set; }
        [JsonProperty("messageDate")]
        public DateTime MessageDate { get; set; }
        [JsonProperty("contents")]
        public string Contents { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("messageType")]
        public MessageType MessageType { get; set; }
    }

}