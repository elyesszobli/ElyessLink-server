using ElyessLink_API.Models;
using System.ComponentModel.DataAnnotations;

namespace ElyessLink_API.DTOs
{
    public class MessageDTO
    {
       
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class MessageGetDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }
        public UserGetDTO UserIsseur { get; set; }
        public UserGetDTO UserReciver { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }
    }
}
