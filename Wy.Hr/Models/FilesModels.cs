using System;
using Newtonsoft.Json;

namespace Wy.Hr.Models
{

    public class FilesModels
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("no")]
        public string No { get; set; }
        [JsonProperty("uploader")]
        public string Uploader { get; set; }
        [JsonProperty("uploadTime")]
        public DateTime UploadTime { get; set; }
    }
}