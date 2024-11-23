namespace ElyessLink_API.DTOs
{
    public class TokenGetDTO
    {
        public string Token { get; set; }
        public DateTime emissionDate { get; set; }
        public DateTime expirationDate { get; set; }
        public UserGetDTO User { get; set; }
    }

    public class TokenDeletDTO
    {
        public string Token { get; set; }
    }
}
