namespace ElyessLink_API.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreat { get; set; }
        public User user { get; set; }
        public string? ImagePath { get; set; }
        public int LikeCount { get; set; } 
    }

}
