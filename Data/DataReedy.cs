using ElyessLink_API.Models;
namespace ElyessLink_API.Data
{
    public class DataReedy
    {
        private readonly AppDbContext _appDbContext;
        public DataReedy(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SeedContext()
        {
            if (await _appDbContext.Database.CanConnectAsync())
            {
                if (!_appDbContext.Users.Any())
                {
                    User newUser1 = new User
                    {
                        Username = "Ines92i",
                        Email = "ines@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("123ines123"),
                        ProfilePicturePath = "images_profile/ines92i.jpg"
                    };
                    User newUser2 = new User
                    {
                        Username = "Yassko93",
                        Email = "yasko@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("123yasko123"),
                        ProfilePicturePath = "images_profile/yasko.jpg"
                    };

                    _appDbContext.Users.Add(newUser1);
                    _appDbContext.Users.Add(newUser2);
                    _appDbContext.SaveChanges();
                }

            }
        }
    }
}
