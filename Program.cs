using static System.Net.Mime.MediaTypeNames;
using ElyessLink_API.Data;
using ElyessLink_API.Services.Mappers;
using ElyessLink_API.Middleware;
using ElyessLink_API.Repositories;
using ElyessLink_API.Models;


var Origins = "AuthorizedApps";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<TokenMapper>();
builder.Services.AddScoped<PostMapper>();
builder.Services.AddScoped<IRepository<User>,UserRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Origins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                          .WithMethods("GET", "POST", "PUT", "DELETE", "OPTION")
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(Origins);



/*
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/auth"), appBuilder =>
{
    appBuilder.UseMiddleware<AuthMiddleware>();
});*/

app.Run();
