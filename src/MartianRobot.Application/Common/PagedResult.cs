using System.Collections.Generic;

namespace MartianRobot.Api.Model
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IReadOnlyList<T> Results { get; set; }

        public PagedResult()
        {
            this.Results = new List<T>();
        }

        public PagedResult(IEnumerable<T> items, int page, int pageSize)
        {
            this.Results = new List<T>(items);
            this.CurrentPage = page;
            this.PageSize = pageSize;
        }
    }
}
