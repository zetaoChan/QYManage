using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Wy.Hr.Common
{
    /// <summary>
    /// 分页内容类
    /// <c></c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class PagedList<T>
    {
        public PagedList()
        {
            Items = new List<T>();
        }
        /// <summary>
        /// 构造函数<c>abc</c>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            TotalPageCount = (int)System.Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPageIndex = pageIndex;
            StartRecordIndex = (CurrentPageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Items.Add(items[i]);
            }
        }
        /// <summary>
        /// 构造函数<c>abc</c>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            Items = items.ToList();
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)System.Math.Ceiling(totalItemCount / (double)pageSize);
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
        }
        [DataMember]
        public IList<T> Items { get; set; }
        [DataMember]
        public int CurrentPageIndex { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public int TotalItemCount { get; set; }
        [DataMember]
        public int TotalPageCount { get; private set; }
        [DataMember]
        public int StartRecordIndex { get; private set; }
        [DataMember]
        public int EndRecordIndex { get; private set; }
    }
}
