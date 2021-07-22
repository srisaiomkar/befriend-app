namespace API.DTOs
{
    public class UserParams : PaginationParams
    {
        public string currentUserName { get; set; }
        public string Gender { get; set; }
    }
}