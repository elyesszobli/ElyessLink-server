using Microsoft.AspNetCore.Mvc;
using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using ElyessLink_API.Services.Mappers;
using Microsoft.AspNetCore.Http;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly TokenMapper _tokenMapper;
        private readonly IHttpContextAccessor _httpReponse;

        public AuthController(AppDbContext appDbContext, TokenMapper tokenMapper, IHttpContextAccessor httpReponse)
        {
            _appDbContext = appDbContext;
            _tokenMapper = tokenMapper;
            _httpReponse = httpReponse;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserAuthDTO _authUser)
        {
            if (_authUser.Email == null || _authUser.Password == "")
            {
                return BadRequest("Rensignz tout les champs");
            }

            var _user = _appDbContext.Users.FirstOrDefault(user => user.Email == _authUser.Email );
            if (_user == null)
            {
                return BadRequest("L'utilisateur n'as pas été trouvé");
            }

            bool validPassword = BCrypt.Net.BCrypt.Verify(_authUser.Password, _user.Password);
            if (!validPassword)
            {
                return Unauthorized("Mot de passe ou nom d'utilisateur érroné");
            }

            var token = new AuthToken
            {
                emissionDate = DateTime.UtcNow,
                expirationDate = DateTime.UtcNow.AddDays(3),
                token = Guid.NewGuid().ToString(),
                user = _user,
            };

            _httpReponse.HttpContext.Response.Cookies.Append("refreshToken", token.token,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });


            _appDbContext.AuthTokens.Add(token);
            await _appDbContext.SaveChangesAsync();
            return Ok(_tokenMapper.TokenToDTO(token));
        }

        [HttpPost("creatuser")]
        public User CreateUser(UserCreateDTO userDto)
        {

            var user = new User()
            {
                Email = userDto.Email,
                Username = userDto.Username,
                Password = userDto.Password,
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Password = hashedPassword;


            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            return user;
        }
    }
}
