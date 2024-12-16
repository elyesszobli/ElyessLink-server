namespace ElyessLink_API.Models
{
    public class Like
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
    }

}
