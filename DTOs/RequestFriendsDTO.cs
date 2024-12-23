namespace ElyessLink_API.DTOs
{
    public class RequestFriendsGetDTO
    {
        public int Id { get; set; }
        public UserGetDTO UserReceiver { get; set; }
        public string Status { get; set; }
        public DateTime DateRequest { get; set; }

    }
}
