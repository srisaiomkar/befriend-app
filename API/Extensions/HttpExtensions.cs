using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int pageNumber, int itemsPerPage,
            int totalPages,int totalItems)
        {
              PaginationHeader paginationHeader = new PaginationHeader(pageNumber,itemsPerPage,totalPages,totalItems);
              response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader));
              response.Headers.Add("Access-Control-Expose-Headers","Pagination"); 
        }
    }
}