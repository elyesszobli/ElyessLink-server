using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using ElyessLink_API.Repositories;
using ElyessLink_API.Services.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRepository<User> _userRepository;
        private readonly UserMapper _userMapper;
        private readonly IHttpContextAccessor _httpReponse;

        public UserController(AppDbContext appDbContext, IRepository<User> userRepository, UserMapper userMapper, IHttpContextAccessor httpReponse)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
            _userMapper = userMapper;
            _httpReponse = httpReponse;
        }

        [HttpGet]
        public List<UserGetDTO> GetAllUsers()
        {
            var users = _appDbContext.Users.ToList();
            return users.Select(user => _userMapper.UserToDTO(user)).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByID(int id)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return BadRequest("Utilisateur introuvable");
            }

            var userDTO = _userMapper.UserToDTO(user);
            return Ok(userDTO);
        }

       /* [HttpGet]
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
        }*/
    }
}

