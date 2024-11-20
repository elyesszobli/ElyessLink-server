namespace ElyessLink_API.Models
{
    public class Message
    {
        int Id { get; set; }
        public string Content { get; set; }
        public User UserIsseur { get; set; }
        public User UserReciver { get; set; }
        public DateTime Created { get; set; }
    }
}
