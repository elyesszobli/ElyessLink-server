using ElyessLink_API.Data;
using ElyessLink_API.Models;
using Microsoft.AspNetCore.Mvc;
using ElyessLink_API.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ElyessLink_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestFriendsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpReponse;
        public readonly RequestFriendsMapper _requestFriendsMapper;

        public RequestFriendsController(AppDbContext appDbContext, IHttpContextAccessor httpReponse, RequestFriendsMapper requestFriendsMapper)
        {
            _appDbContext = appDbContext;
            _httpReponse = httpReponse;
            _requestFriendsMapper = requestFriendsMapper;
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest(int _idReceiver)
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

            var Receiver = _appDbContext.Users.FirstOrDefault(t => t.Id == _idReceiver);
            if (Receiver == null)
            {
                return BadRequest("Destinataire non trouvé");
            }

            // Vérification si une demande existe déjà entre les deux utilisateurs
            var existingRequest = _appDbContext.RequestFriends
                .FirstOrDefault(r => (r.UserIssuer.Id == _user.Id && r.UserReceiver.Id == Receiver.Id) ||
                                    (r.UserIssuer.Id == Receiver.Id && r.UserReceiver.Id == _user.Id));

            if (existingRequest != null)
            {
                return BadRequest("Une demande d'ami existe déjà entre ces deux utilisateurs");
            }

            var requestFriends = new RequestFriends
            {
                UserIssuer = _user,
                UserReceiver = Receiver,
                Status = "OnHold",
                DateRequest = DateTime.UtcNow,
            };

            _appDbContext.RequestFriends.Add(requestFriends);
            await _appDbContext.SaveChangesAsync();
            return Ok("Invitation a été bien envoyée");
        }

        [HttpPut("{requestId}")]
        public async Task<IActionResult> AcceptRequest(int requestId)
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

            var requestFriends = _appDbContext.RequestFriends
                .Include(r => r.UserIssuer)
                .Include(r => r.UserReceiver)
                .FirstOrDefault(r => r.Id == requestId);

            if (requestFriends == null)
            {
                return BadRequest("Demande d'ami non trouvée");
            }

            if (requestFriends.UserReceiver.Id != _user.Id)
            {
                return BadRequest("Vous n'êtes pas autorisé à accepter cette demande");
            }

            requestFriends.Status = "Accepted";
            //requestFriends.DateAccepted = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
            return Ok("Demande d'ami acceptée");
        }

        [HttpDelete("{requestId}")]
        public async Task<IActionResult> DeleteRequest(int requestId)
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

            var requestFriends = _appDbContext.RequestFriends
                .Include(r => r.UserIssuer)
                .Include(r => r.UserReceiver)
                .FirstOrDefault(r => r.Id == requestId);

            if (requestFriends == null)
            {
                return BadRequest("Demande d'ami non trouvée");
            }

            if (requestFriends.UserIssuer.Id != _user.Id && requestFriends.UserReceiver.Id != _user.Id)
            {
                return BadRequest("Vous n'êtes pas autorisé à supprimer cette demande");
            }

            _appDbContext.RequestFriends.Remove(requestFriends);
            await _appDbContext.SaveChangesAsync();
            return Ok("Demande d'ami supprimée");
        }

        [HttpGet]
        public async Task<IActionResult> GetReceivedRequests()
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

            var receivedRequests = _appDbContext.RequestFriends
                .Include(r => r.UserIssuer)
                .Include(r => r.UserReceiver)
                .Where(r => r.UserReceiver.Id == _user.Id && r.Status == "OnHold")
                .ToList();

            var receivedRequestsDTO = receivedRequests.Select(request => _requestFriendsMapper.RequestFriendsToDTO(request)).ToList();

            return Ok(receivedRequestsDTO);
        }
    }
}
