using System;
using Newtonsoft.Json;
using Wy.Hr.Common;

namespace Wy.Hr.Models
{

    public class StaffPagedArgs : PagedArgs
    {
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
    }

    public class StaffModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("positionName")]
        public string PositionName { get; set; }
        [JsonProperty("departmentId")]
        public int? DepartmentId { get; set; }
        [JsonProperty("positionId")]
        public int? PositionId { get; set; }
        [JsonProperty("positionGrade")]
        public PositionGrade PositionGrade { get; set; }
        [JsonProperty("sex")]
        public Sex Sex { get; set; }
        [JsonProperty("iDCardNum")]
        public string IDCardNum { get; set; }
        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }
        [JsonProperty("nation")]
        public Nation Nation { get; set; }
        [JsonProperty("nativePlace")]
        public NativePlace NativePlace { get; set; }
        [JsonProperty("education")]
        public Education Education { get; set; }
        [JsonProperty("graduatingAcademy")]
        public string GraduatingAcademy { get; set; }
        [JsonProperty("maritalStatus")]
        public MaritalStatus MaritalStatus { get; set; }
        [JsonProperty("entryTime")]
        public DateTime EntryTime { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }

}