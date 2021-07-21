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
              // API response which is in PascalCase will be converted to JSON camelCase by default
              // But will not be converted while adding headers, so converting it to camelCase using the options
              var options = new JsonSerializerOptions
              {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
              };
              response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader,options));
              response.Headers.Add("Access-Control-Expose-Headers","Pagination"); 
        }
    }
}