﻿using Microsoft.AspNetCore.Mvc;
using ElyessLink_API.Data;
using ElyessLink_API.DTOs;
using ElyessLink_API.Models;
using ElyessLink_API.Services.Mappers;
using Microsoft.AspNetCore.Http;

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
            if(userToken == null)
            {
                return BadRequest("token non recupere");
            }
            var token = _appDbContext.AuthTokens.FirstOrDefault(t => t.token == userToken);
            if (token == null)
            {
                return BadRequest("token non recupere");
            }

            var _user = _appDbContext.Users.FirstOrDefault(t => t.Id == token.user.Id);

            if(_user == null )
            {
                return BadRequest("user non recupere");
            }

            if (_post.Content == null)
            {
                return BadRequest("Rensignz tout les champs");
            }

            var post = new Post
            {
                Content = _post.Content,
                DateCreat = DateTime.Now,
                user = _user,
            };

            _appDbContext.Posts.Add(post);
            await _appDbContext.SaveChangesAsync();
            return Ok(_postMapper.PostToDTO(post));

        }

    }
}
