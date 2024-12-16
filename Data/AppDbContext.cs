using Microsoft.EntityFrameworkCore;
using ElyessLink_API.Models;

namespace ElyessLink_API.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("bdd"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }   
    }
}
