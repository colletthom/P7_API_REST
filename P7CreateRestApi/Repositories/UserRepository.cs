using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        public LocalDbContext _context { get; }

        public UserRepository(LocalDbContext context)
        {
            _context = context;
        }

        public User FindByUserName(string userName)
        {
            return _context.Users.Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public async Task<List<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddUser(User user)
        {
            var _user = new User 
            { 
                UserName = user.UserName,
                Password = user.Password,
                Fullname = user.Fullname,
                Role = user.Role
            };

            _context.Users.Add(_user);
            await _context.SaveChangesAsync();
            return _user;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UpdateUserById(int id, User user)
        {
            var _user = _context.Users.Find(id);
            if (_user == null)
            {
                return false;
            }

            _user.UserName = user.UserName;
            _user.Password = user.Password;
            _user.Fullname = user.Fullname;
            _user.Role = user.Role;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Or throw an exception
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}