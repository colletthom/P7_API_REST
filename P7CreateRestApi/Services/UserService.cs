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

        public async Task<Object> AddUser(RegisterModel model)
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
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                return new { Errors = result.Errors };
            }
        }

        public async Task<object> UpdateUserById(int id, UpdateModel user)
        {
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.FullName = user.FullName;
            existingUser.Role = user.Role;
            existingUser.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.Password))
            {
                var newPasswordHash = _passwordHasher.HashPassword(existingUser, user.Password);
                existingUser.PasswordHash = newPasswordHash;
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var result = await _userManager.ResetPasswordAsync(existingUser, token, user.Password);
                if (!result.Succeeded)
                {
                    return new { Errors = result.Errors };
                }
            }
            var updateResult = await _userManager.UpdateAsync(existingUser);

            if (!updateResult.Succeeded)
            {
                return new { Errors = updateResult.Errors };
            }

            return existingUser;
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