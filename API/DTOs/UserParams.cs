namespace API.DTOs
{
    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }  
        public int minAge { get; set; } = 18;
        public int maxAge { get; set; } = 200;
        public string OrderBy { get; set; } = "LastActive";
    }
}