namespace ElyessLink_API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
        public User UserIsseur { get; set; }
        public User UserReciver { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }
    }
}
