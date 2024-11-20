using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElyessLink_API.Data;

namespace ElyessLink_API.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var db = context.RequestServices.GetService<AppDbContext>();

            var authorizationHeader = context.Request.Headers["Authorization"];

            string accessToken = string.Empty;

            // traitement de chaine de caractères
            if (authorizationHeader.ToString().StartsWith("Bearer"))
            {
                accessToken = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();
            }

            if (accessToken == string.Empty)
            {
                context.Response.Redirect("Errors/");
            }

            var test = db.AuthTokens.Include(u => u.user).FirstOrDefault((token) => token.token == accessToken);

            if (test == null)
            {
                context.Response.Redirect("Errors/");
            }

            //Console.WriteLine(test.User.Id);

            // Vérifier que le token est valide


            //Console.WriteLine("Token : " + accessToken);

            await _next(context);
        }
    }
}
