using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wy.Hr.Common
{

    public enum TaskStatus {
        未开始 = 0,
        进行中 = 1,
        已完成 = 2
    }

    public enum DepartmentGrade
    {
        公司 = 0,
        部门 = 1
    }

    /// <summary>
    /// 学历
    /// </summary>
    public enum Education
    {
        高中 = 0,
        大专 = 1,
        本科 = 2,
        硕士 = 3,
        MBA = 4,
        EMBA = 5
    }

    /// <summary>
    /// 婚姻状况
    /// </summary>
    public enum MaritalStatus
    {
        未婚 = 0,
        已婚 = 1,
        离异 = 2
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        男 = 0,
        女 = 1
    }

    /// <summary>
    /// 职位等级
    /// </summary>
    public enum PositionGrade
    {
        Z1 = 1,
        Z2 = 2,
        Z3 = 3,
        Z4 = 4,
        Z5 = 5,
        Z6 = 6,
        Z7 = 7
    }

    /// <summary>
    /// 民族
    /// </summary>
    public enum Nation
    {
        汉族 = 0,
        满族 = 1,
        苗族 = 2,
        瑶族 = 3,
        壮族 = 4,
        傣族 = 5,
        回族 = 6,
        藏族 = 7,
        其它 = 8
    }

    /// <summary>
    /// 籍贯
    /// </summary>
    public enum NativePlace
    {

        河北省 = 0,
        山东省 = 1,
        辽宁省 = 2,
        黑龙江省 = 3,
        吉林省 = 4,
        甘肃省 = 5,
        青海省 = 6,
        河南省 = 7,
        江苏省 = 8,
        湖北省 = 9,
        湖南省 = 10,
        山西省 = 11,
        江西省 = 12,
        浙江省 = 13,
        广东省 = 14,
        云南省 = 15,
        福建省 = 16,
        台湾省 = 17,
        海南省 = 18,
        四川省 = 19,
        陕西省 = 20,
        贵州省 = 21,
        重庆市 = 22,
        北京市 = 23,
        上海市 = 24,
        天津市 = 25,
        广西 = 26,
        内蒙古 = 27,
        西藏 = 28,
        新疆 = 29,
        宁夏 = 30,
        澳门 = 31,
        香港 = 32,
        安徽省 = 33,
        其他 = 34
    }

    public enum MessageStatus
    { 
        未读 = 0,
        已读 = 1
    }

    public enum MessageType
    {
        系统消息 = 0,
        用户消息 = 1
    }

    public enum ArticleType
    {
        新闻 = 0,
        通知 = 1
    }
    
}