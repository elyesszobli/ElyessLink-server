using static System.Net.Mime.MediaTypeNames;
using ElyessLink_API.Data;
using ElyessLink_API.Services.Mappers;
using ElyessLink_API.Middleware;
using ElyessLink_API.Repositories;
using ElyessLink_API.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var Origins = "AuthorizedApps";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<TokenMapper>();
builder.Services.AddScoped<PostMapper>();
builder.Services.AddScoped<MessageMapper>();
builder.Services.AddScoped<RequestFriendsMapper>();
builder.Services.AddScoped<DataReedy>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();

// Configuration des cookies pour l'authentification
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "ElyessLink-cookie";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = true;
    });

// Configuration des CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Origins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                                .AllowAnyHeader()
                                .AllowCredentials();
                      });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DataReedy>();
await seeder.SeedContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(Origins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.UseWhen(context => !context.Request.Path.StartsWithSegments("/auth"), appBuilder =>
{
    appBuilder.UseMiddleware<AuthMiddleware>();
});

app.Run();
