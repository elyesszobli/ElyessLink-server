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
    public class PostController : ControllerBase
    {
        private readonly PostMapper _postMapper;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpReponse;

        public PostController (AppDbContext appDbContext, IHttpContextAccessor httpReponse , PostMapper postMapper)
        {
            _appDbContext = appDbContext;
            _httpReponse = httpReponse;
            _postMapper = postMapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreatPost(PostAddDTO _post)
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

            if (string.IsNullOrEmpty(_post.Content))
            {
                return BadRequest("Rensignz tout les champs");
            }

            string? imagePath = null;
            if (_post.Image != null && _post.Image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images_post", _post.Image.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await _post.Image.CopyToAsync(stream);
                }
                imagePath = "/images_post/" + _post.Image.FileName; 
            }

            var post = new Post
            {
                Content = _post.Content,
                DateCreat = DateTime.UtcNow,
                user = _user,
                ImagePath = imagePath,
                LikeCount = 0
            };

            _appDbContext.Posts.Add(post);
            await _appDbContext.SaveChangesAsync();
            return Ok(_postMapper.PostToDTO(post));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            {
                var posts = await _appDbContext.Posts
                    .Include(p => p.user)
                    .ToListAsync();

                var postDTOs = posts.Select(post => _postMapper.PostToDTO(post)).ToList();

                return Ok(postDTOs);
            }
        }



    }
}
