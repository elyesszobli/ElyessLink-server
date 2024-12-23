using ElyessLink_API.DTOs;
using ElyessLink_API.Models;

namespace ElyessLink_API.Services.Mappers
{
    public class RequestFriendsMapper
    {
        public RequestFriendsGetDTO RequestFriendsToDTO(RequestFriends request)
        {
            return new RequestFriendsGetDTO
            {
                Id = request.Id,
                Status = request.Status,
                DateRequest = request.DateRequest,
                UserReceiver = new UserGetDTO
                {
                    Id = request.UserReceiver.Id,
                    Username = request.UserReceiver.Username,
                    Email = request.UserReceiver.Email,
                }

            };
        }
    }
}
