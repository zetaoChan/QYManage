using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wy.Hr.Models
{
    public class PagedArgs
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class PagedResult<T>
    {
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
        [JsonProperty("totalPageCount")]
        public int TotalPageCount { get; set; }
        [JsonProperty("items")]
        public IList<T> Items { get; set; }
    }

    public class SingleModelArgs
    {
        public int Id { get; set; }
    }

    public class BatchModelArgs
    {
        public int[] Ids { get; set; }
    }

    /// <summary>
    /// 导出属性模型
    /// </summary>
    public class ParameterModel
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}