using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace MartianRobot.Domain.SeedWork
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Results { get; set; }
        public T Result { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }

        public PagedResult(IEnumerable<T> items, int page, int pageSize)
        {
            Results = new List<T>(items);
            CurrentPage = page;
            PageSize = pageSize;
        }

        public PagedResult(T items, int page, int pageSize)
        {
            Result = items;
            CurrentPage = page;
            PageSize = pageSize;
        }


        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResult(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecords = count;

            Results = new List<T>(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>(items, count, pageIndex, pageSize);
        }

        public static PagedResult<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<T>(items, count, pageIndex, pageSize);
        }


    }
}
