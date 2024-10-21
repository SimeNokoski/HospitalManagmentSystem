using HospitalManagementSystem.DataAccess.Interfaces;
using HospitalManagementSystem.Domain.Models;

namespace HospitalManagementSystem.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalManagementSystemDbContext _context;
        public UserRepository(HospitalManagementSystemDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();

        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
        }

        public User LoginUser(string username, string hashedPassword)
        {
            return _context.Users.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower()
            && x.Password == hashedPassword);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
