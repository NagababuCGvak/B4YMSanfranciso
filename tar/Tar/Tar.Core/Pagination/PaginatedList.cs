using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Tar.Core.Pagination
{
    [DataContract]
    public class PaginatedList<T>//: IPaginatedList<T>
    {
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public int TotalPageCount { get; set; }
        [DataMember]
        public List<T> List { get; set; }

        public PaginatedList()
            :this(Enumerable.Empty<T>(), new PaginationInfoView())
        {   
        }

        public PaginatedList(IEnumerable<T> source, PaginationInfoView paginationInfo)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (paginationInfo == null) throw new ArgumentNullException("paginationInfo");

            PageIndex = paginationInfo.PageIndex;
            PageSize = paginationInfo.PageSize;
            TotalCount = source.Count();
            TotalPageCount = (int) Math.Ceiling(TotalCount/(double) PageSize);
            if (PageIndex > TotalPageCount || PageIndex < 1) PageIndex = 1;

            List = source.Skip((PageIndex - 1)*PageSize).Take(PageSize).ToList();
        }
        [DataMember]
        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
            set { }
        }
        [DataMember]
        public bool HasNextPage
        {
            get { return (PageIndex < TotalPageCount); }
            set { }
        }
    }
}
