using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly Token _tokenService;

        public HomeController(UserManager<User> userManager, Token tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        /*
        [HttpPost("Login")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            var user = new User { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // L'inscription a réussi
                return Ok("User created successfully");
            }
            else
            {
                // L'inscription a échoué
                return BadRequest(result.Errors);
            }
        }*/
        [HttpGet]
        public IActionResult Get()
        {
            //page d'accueil d'inscrition
            return RedirectToAction("Register", "Account", new { area = "Identity" });
            //return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            // Logique d'inscription et obtention du token
            return Ok();
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("Admin")]
        public IActionResult Admin()
        {
            //return Ok();
            return RedirectToAction("Index", "Swagger");
        }
    }
}