using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using ElyessLink_API.Services.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpReponse;
        public readonly MessageMapper _messageMapper;
        
        public MessageController(AppDbContext appDbContext, IHttpContextAccessor httpReponse, MessageMapper messageMapper)
        {
            _appDbContext = appDbContext;
            _httpReponse = httpReponse;
            _messageMapper = messageMapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreatMessage(int ReciverId , MessageDTO _message)
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

            
            
            string? imagePath = null;

            if (_message.Image != null && _message.Image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images_message", _message.Image.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await _message.Image.CopyToAsync(stream);
                }
                imagePath = "/images_message/" + _message.Image.FileName;
            }

            if(_message.Content == null && _message.Image == null)
            {
                return  BadRequest("Rensignz tout les champs");
            }

            var Reciver = _appDbContext.Users.FirstOrDefault(t => t.Id == ReciverId);
            if (Reciver == null)
            {
                return BadRequest("Reciver non trouve");
            }

            var message = new Message
            {
                Content = _message.Content,
                ImagePath = imagePath,
                UserIsseur = _user,
                UserReciver = Reciver,
                Created = DateTime.UtcNow,
                IsRead = false,
            };

            _appDbContext.Messages.Add(message);
            await _appDbContext.SaveChangesAsync();
            return Ok(_messageMapper.MessageToDTO(message));
        }
    }
}
