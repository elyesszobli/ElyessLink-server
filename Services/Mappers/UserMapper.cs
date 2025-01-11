using ElyessLink_API.DTOs;
using ElyessLink_API.Models;

namespace ElyessLink_API.Services.Mappers
{
    public class UserMapper
    {
        public UserGetDTO UserToDTO(User user)
        {
            return new UserGetDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfilePicturePath = user.ProfilePicturePath,
            };
        }
    }
}
