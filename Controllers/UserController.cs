using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UserController (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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
