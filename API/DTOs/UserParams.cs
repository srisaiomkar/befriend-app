namespace API.DTOs
{
    public class UserParams : PaginationParams
    {
        public string currentUserName { get; set; }
        public string Gender { get; set; }  
        public int minAge { get; set; } = 18;
        public int maxAge { get; set; } = 200;
    }
}