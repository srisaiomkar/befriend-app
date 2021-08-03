namespace API.Entities
{
    public class Like
    {
        public AppUser LikedByUser { get; set; }
        public int LikedByUserId { get; set; }
        public AppUser LikedUser { get; set; }
        public int LikedUserId { get; set; }
    }
}