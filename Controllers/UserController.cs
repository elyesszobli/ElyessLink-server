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

        [HttpPut("updateprofilepicture")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
        {
            var userToken = _httpReponse.HttpContext.Request.Cookies["ElyessLink-cookie"];
            if (userToken == null)
            {
                return BadRequest("Token non récupéré");
            }

            var _token = _appDbContext.AuthTokens.Include(u => u.user).FirstOrDefault(t => t.token == userToken);
            if (_token == null)
            {
                return BadRequest("Token non récupéré");
            }

            var _user = _appDbContext.Users.FirstOrDefault(t => t.Username == _token.user.Username);
            if (_user == null)
            {
                return BadRequest("Utilisateur non récupéré");
            }

            if (profilePicture == null || profilePicture.Length == 0)
            {
                return BadRequest("Aucune photo de profil n'a été téléchargée");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images_profile", profilePicture.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            _user.ProfilePicturePath = "/images_profile/" + profilePicture.FileName;
            _appDbContext.Users.Update(_user);
            await _appDbContext.SaveChangesAsync();

            return Ok("Photo de profil mise à jour avec succès");
        }

    }
}

