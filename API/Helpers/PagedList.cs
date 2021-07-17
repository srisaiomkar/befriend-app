using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items,int pageNumber, int itemsPerPage, int totalItems)
        {
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;
            TotalPages = (int) Math.Ceiling(totalItems/(double)itemsPerPage);
            TotalItems = totalItems;
            AddRange(items);
        }

        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,int pageNumber,int itemsPerPage){
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber -1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
            return new PagedList<T>(items,pageNumber,itemsPerPage,count);
        }
    }
}