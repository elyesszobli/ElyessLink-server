using ElyessLink_API.DTOs;
using ElyessLink_API.Models;

namespace ElyessLink_API.Services.Mappers
{
    public class MessageMapper
    {
        public MessageGetDTO MessageToDTO(Message message)
        {
            return new MessageGetDTO
            {
                Content = message.Content,
                ImagePath = message.ImagePath,
                Created = message.Created,
                IsRead = message.IsRead,
                UserIsseur = new UserGetDTO
                {
                    Id = message.UserIsseur.Id,
                    Username = message.UserIsseur.Username,
                    Email = message.UserIsseur.Email,
                },
                UserReciver = new UserGetDTO
                {
                    Id = message.UserReciver.Id,
                    Username = message.UserReciver.Username,
                    Email = message.UserReciver.Email,
                }
            };
        }
    }
}
