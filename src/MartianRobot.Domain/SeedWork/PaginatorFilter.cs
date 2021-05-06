using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobot.Domain.SeedWork
{
    public class PaginationFilter
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public List<int> Ids { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
