using System.Collections.Generic;

namespace PTMS.Common
{
    public class PageResult<T>
    {
        public List<T> Page { get; set; }
        public int TotalCount { get; set; }

        public PageResult(List<T> page, int totalCount)
        {
            Page = page;
            TotalCount = totalCount;
        }
    }
}
