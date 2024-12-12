using ElyessLink_API.Models;

namespace ElyessLink_API.DTOs
{
    public class PostAddDTO
    {
        public string Content { get; set; }

    }

    public class PostGetDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
       
        public DateTime DateCreat { get; set; }

        public UserGetDTO user { get; set; }

    }
}
