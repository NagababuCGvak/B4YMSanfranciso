using System;
using System.Collections.Generic;
using System.Linq;

namespace Tar.Core.Pagination
{
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> source, int pageIndex)
            : this(source, pageIndex, 10)
        {
        }

        public PaginatedList(IEnumerable<T> source)
            : this(source, 1, 10)
        {
        }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (source == null) throw new ArgumentNullException("source");
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }
    }


}
