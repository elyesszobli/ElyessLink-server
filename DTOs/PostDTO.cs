using ElyessLink_API.Models;
using System.ComponentModel.DataAnnotations;

namespace ElyessLink_API.DTOs
{
    public class PostAddDTO
    {
        [Required]
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class PostGetDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
       
        public DateTime DateCreat { get; set; }

        public string? ImagePath { get; set; }
        public int LikeCount { get; set; }

        public UserGetDTO user { get; set; }

    }
}
