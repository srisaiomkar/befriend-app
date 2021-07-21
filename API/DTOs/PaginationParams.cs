namespace API.DTOs
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } =1;
        private const int _maxItemsPerPage = 50;
        private int _itemsPerPage = 10;
        public int ItemsPerPage{
            get => _itemsPerPage;
            set => _itemsPerPage = (value > _maxItemsPerPage? _maxItemsPerPage : value);
        }
    }
}