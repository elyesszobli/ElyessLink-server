namespace ElyessLink_API.DTOs
{
    public class TokenGetDTO
    {
        public string Token { get; set; }
        public DateTime emissionDate { get; set; }
        public DateTime expirationDate { get; set; }
        public UserGetDTO User { get; set; }
    }
}
