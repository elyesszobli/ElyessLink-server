using ElyessLink_API.DTOs;
using ElyessLink_API.Models;

namespace ElyessLink_API.Services.Mappers
{
    public class TokenMapper
    {
        public TokenGetDTO TokenToDTO(AuthToken token)
        {
            return new TokenGetDTO
            {
                emissionDate = token.emissionDate,
                expirationDate = token.expirationDate,
                Token = token.token,
                User = new UserGetDTO
                {
                    Id = token.user.Id,
                    Username = token.user.Username,
                    Email = token.user.Email,
                }
            };
        }
    }
}
