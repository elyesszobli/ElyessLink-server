namespace ElyessLink_API.Models
{
    public class AuthToken
    {
        public int Id { get; set; }
        public DateTime emissionDate { get; set; }
        public DateTime expirationDate { get; set; }
        public string token { get; set; }
        public User user { get; set; }
    }
}
