using Microsoft.AspNetCore.Mvc;
using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using ElyessLink_API.Services.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly TokenMapper _tokenMapper;
        private readonly IHttpContextAccessor _httpReponse;
        private readonly UserMapper _userMapper;

        public AuthController(AppDbContext appDbContext, TokenMapper tokenMapper, IHttpContextAccessor httpReponse, UserMapper userMapper)
        {
            _appDbContext = appDbContext;
            _tokenMapper = tokenMapper;
            _httpReponse = httpReponse;
            _userMapper = userMapper;
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

            _httpReponse.HttpContext.Response.Cookies.Append("ElyessLink-cookie", token.token,
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


        [HttpDelete]
        public async Task<IActionResult> SignOut()
        {
            var userToken = _httpReponse.HttpContext.Request.Cookies["ElyessLink-cookie"];

            var token = _appDbContext.AuthTokens.FirstOrDefault(t => t.token == userToken);

            if(token == null) { return BadRequest("Token not found"); }

            _httpReponse.HttpContext.Response.Cookies.Append("ElyessLink-cookie", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            _appDbContext.AuthTokens.Remove(token);
            await _appDbContext.SaveChangesAsync();
            return Ok("Token deleted successfully");
        }

        [HttpPost("creatuser")]
        public User CreateUser(UserCreateDTO userDto)
        {
            var user = new User()
            {
                Email = userDto.Email,
                Username = userDto.Username,
                Password = userDto.Password,
                ProfilePicturePath = "default-profile-picture.jpg" 
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Password = hashedPassword;

            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            return user;
        }


        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            var userToken = _httpReponse.HttpContext.Request.Cookies["ElyessLink-cookie"];

            var token = _appDbContext.AuthTokens.FirstOrDefault(t => t.token == userToken);

            if (_httpReponse.HttpContext.Request.Cookies["ElyessLink-cookie"] != null && token != null)
            {
                return Ok(new { isLoggedIn = true });
            }

            return Ok(new { isLoggedIn = false });
        }

        [HttpGet("user-information")]
        public IActionResult GetUserInformation()
        {
            var userToken = _httpReponse.HttpContext.Request.Cookies["ElyessLink-cookie"];
            if (userToken == null)
            {
                return BadRequest("token non recupere");
            }
            var _token = _appDbContext.AuthTokens.Include(u => u.user).FirstOrDefault(t => t.token == userToken);

            if (_token == null)
            {
                return BadRequest("token non recupere");
            }

            var _user = _appDbContext.Users.FirstOrDefault(t => t.Username == _token.user.Username);

            if (_user == null)
            {
                return BadRequest("user non recupere");
            }

            var userDTO = _userMapper.UserToDTO(_user);
            return Ok(userDTO);
        }


    }
}
