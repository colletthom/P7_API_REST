using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Models;
using System.Data;

namespace Dot.Net.WebApi.Repositories
{
    public class UserService
    {
        public LocalDbContext _context { get; }
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(LocalDbContext context, UserManager<User> userManager, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public User FindByUserName(string userName)
        {
            return _context.Users.Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public async Task<User> AddUser(RegisterModel model)
        {

            // Création du compte utilisateur
            var user = new User
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Role = "User"
            };
            var passwordHash = _passwordHasher.HashPassword(user, model.Password);
            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserById(int id, User user)
        {

            var _user = _context.Users.Find(id);

            if (_user != null)
            {
                _user.FullName = user.FullName; 
                _user.Role = user.Role;
                _user.UserName = user.UserName;
                _user.PasswordHash = user.PasswordHash;

                var result = await _userManager.UpdateAsync(_user);
            }


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