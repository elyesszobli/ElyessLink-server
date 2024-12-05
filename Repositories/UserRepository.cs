using ElyessLink_API.Data;
using ElyessLink_API.Models;

namespace ElyessLink_API.Repositories
{
    public class UserRepository:IRepository<User>
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User GetById(int id)
        {
            return _appDbContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _appDbContext.Users.ToList();
        }
        public User Create(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            return user;
        }

        public void Delete(User user)
        {
            _appDbContext.Users.Remove(user);
            _appDbContext.SaveChanges();
        }

    }
}
