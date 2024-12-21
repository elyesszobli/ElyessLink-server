namespace ElyessLink_API.Models
{
    public class RequestFriends
    {
        public int Id { get; set; }
        public User UserIssuer { get; set; }
        public User UserReceiver { get; set; }
        public string Status { get; set; }
        public DateTime DateRequest { get; set; }
    }
}
