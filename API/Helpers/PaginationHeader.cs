namespace API.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int pageNumber, int itemsPerPage, int totalPages, int totalItems)
        {
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}