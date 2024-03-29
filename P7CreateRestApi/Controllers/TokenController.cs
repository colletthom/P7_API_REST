using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
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
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly LogService _logService;

        public TokenController(UserManager<User> userManager, IConfiguration configuration, IPasswordHasher<User> passwordHasher, LogService logService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _logService = logService;
        }
        /*
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
        }*/

        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel model)
        {
            // Vérifie les informations d'identification de l'utilisateur
            var user = await _userManager.FindByNameAsync(model.UserName);
            string logDescription = $"{model.UserName} Description: La création de Token a échouée";

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Générer le token JWT
                var token = GenerateJwtToken(user);
                logDescription = $"{model.UserName} Description: La création de Token a réussie";
                await _logService.CreateLog(HttpContext, 1, 7, logDescription);

                return Ok(new { Token = token });
            }
            await _logService.CreateLog(HttpContext, 1, 7, logDescription);
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
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(100),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}