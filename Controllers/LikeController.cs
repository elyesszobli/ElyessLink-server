using ElyessLink_API.Data;
using ElyessLink_API.Models;
using ElyessLink_API.Services.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpReponse;

        public LikeController(AppDbContext appDbContext, IHttpContextAccessor httpReponse)
        {
            _appDbContext = appDbContext;
            _httpReponse = httpReponse;
        }

        [HttpPost("/like")]
        public async Task<IActionResult> LikePost(int postId)
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

            var post = await _appDbContext.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var existingLike = await _appDbContext.Likes
                .FirstOrDefaultAsync(l => l.Post.Id == postId && l.User.Id == _user.Id);

            if (existingLike != null)
            {
                return BadRequest("You have already liked this post");
            }

            var like = new Like
            {
                Post = post,
                User = _user
            };

            _appDbContext.Likes.Add(like);
            post.LikeCount++;
            await _appDbContext.SaveChangesAsync();

            return Ok(new { message = "Post liked successfully", likeCount = post.LikeCount });
        }

        [HttpPost("/dislike")]
        public async Task<IActionResult> UnlikePost(int postId)
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

            var post = await _appDbContext.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var existingLike = await _appDbContext.Likes
                .FirstOrDefaultAsync(l => l.Post.Id == postId && l.User.Id == _user.Id);

            if (existingLike == null)
            {
                return BadRequest("You have not liked this post");
            }

            _appDbContext.Likes.Remove(existingLike);
            post.LikeCount--;
            await _appDbContext.SaveChangesAsync();

            return Ok(new { message = "Post unliked successfully", likeCount = post.LikeCount });
        }
    }
}
