using ElyessLink_API.DTOs;
using ElyessLink_API.Models;

namespace ElyessLink_API.Services.Mappers
{
    public class PostMapper
    {
        public PostGetDTO PostToDTO(Post post)
        {
            return new PostGetDTO
            {
                Content = post.Content,
                DateCreat = DateTime.Now,
                ImagePath = post.ImagePath,
                LikeCount = post.LikeCount,
                user = new UserGetDTO
                {
                    Id = post.user.Id,
                    Username = post.user.Username,
                    Email = post.user.Email,
                }
            };
        }
    }
}
