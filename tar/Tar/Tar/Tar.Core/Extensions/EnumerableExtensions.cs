using System.Collections.Generic;
using Tar.Core.Pagination;

namespace Tar.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IPaginatedList<T> AsPagination<T>(this IEnumerable<T> source, int pageIndex)
        {
            return new PaginatedList<T>(source, pageIndex);
        }
        public static IPaginatedList<T> AsPagination<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(source, pageIndex, pageSize);
        }
    }
}
