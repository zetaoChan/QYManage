using System;
using Newtonsoft.Json;
using Cn.QYManage.Common;

namespace Cn.QYManage.Models
{

    public class ArticlePagedArgs : PagedArgs
    {
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class ArticleModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        public ArticleType Type { get; set; }
        [JsonProperty("contents")]
        public string Contents { get; set; }
        [JsonProperty("addTime")]
        public DateTime AddTime { get; set; }
        [JsonProperty("addUser")]
        public string AddUser { get; set; }
    }

    public class ArticleAddModel
    {
        public string Title { get; set; }
        public ArticleType Type { get; set; }
        public string Contents { get; set; }
    }

    public class ArticleEditModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ArticleType Type { get; set; }
        public string Contents { get; set; }
    }

    public class ArticleBatchDelArgs
    {
        public int[] Ids { get; set; }
    }

    public class TopArticleArgs
    {
        public ArticleType Type { get; set; }
        public int Top { get; set; }
    }

}