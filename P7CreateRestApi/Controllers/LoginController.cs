using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginController(UserManager<User> userManager, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _userManager = userManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            // Validation des données d'inscription
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Création du compte utilisateur
            var user = new User
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Role = "User"
            };
            var passwordHash = _passwordHasher.HashPassword(user, model.Password);
            user.PasswordHash = passwordHash;

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User created successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel model)
        {
            // Vérifie les informations d'identification de l'utilisateur
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Générer le token JWT
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
                // Ajoutez d'autres revendications personnalisées si nécessaire
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}