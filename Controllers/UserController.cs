using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using ElyessLink_API.Repositories;
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

        public UserController (AppDbContext appDbContext , IRepository<User> userRepository)
        {
            _appDbContext = appDbContext;
            _userRepository = userRepository;
        }

        [HttpGet]
        public List<User> getAllUsers()
        {
            return _appDbContext.Users.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByID(int id)
        {
            var response = _appDbContext.Users.FirstOrDefault(user => user.Id == id);

            if (response == null)
            {
                return BadRequest("utilisateur introuvable");
            }
            else
            {
                return Ok(response);
            }
        }

        

    }
}
