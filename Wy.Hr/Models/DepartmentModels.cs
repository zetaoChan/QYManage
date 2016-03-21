using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Wy.Hr.Common;
using Wy.Hr.Data;

namespace Wy.Hr.Models
{

    public class DepartmentPagedArgs : PagedArgs
    {
        public string DepartmentName { get; set; }
        public int? ParentId { get; set; }
    }

    /// <summary>
    /// 树节点模型
    /// </summary>
    public class TreeItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("i")]
        public int I { get; set; }
        [JsonProperty("children")]
        public IList<TreeItem> Children { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }

    public class DepartmentModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("no")]
        public string No { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
        [JsonProperty("parentName")]
        public string ParentName { get; set; }
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        [JsonProperty("grade")]
        public DepartmentGrade Grade { get; set; }
        [JsonIgnore]
        public Department ParentDept { 
            set {
                var dept = value;
                while (dept != null)
                {
                    if (dept.Grade == DepartmentGrade.公司)
                    {
                        CompanyName = dept.Name;
                    }
                    dept = dept.ParentDepartment;
                }
                
            } 
        }
    }

}