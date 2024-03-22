using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Models;
using System.Diagnostics;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userRepository;
        private readonly LocalDbContext _context;
        private readonly LogService _logService;
        public UserController(UserService userRepository, LocalDbContext context, LogService logService)
        {
            _userRepository = userRepository;
            _context = context;
            _logService = logService;
        }
        /*
        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            return Ok();
        }

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _userRepository.Add(user);

            return Ok();
        }
        */

        [HttpPost]
        //[Route("add")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]RegisterModel user)
        {
            string logDescription = "Le AddUser a réussi";
            if (!ModelState.IsValid)
             {
                logDescription = "Le AddUser a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 1, 6, logDescription);
                return BadRequest(ModelState);
             }

            var addUser = await _userRepository.AddUser(user);
            
            if (addUser is User) 
            {
                await _logService.CreateLog(HttpContext, 1, 6, logDescription);
                return Ok(addUser);
            }
            else
            {
                return BadRequest(addUser);
            }
        }

        [HttpGet]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var _user = await _context.Users.ToListAsync();
            string logDescription = "Le GetAll a réussi";

            await _logService.CreateLog(HttpContext, 2, 6, logDescription);
            return Ok(_user);
        }

        [HttpGet]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            string logDescription = "Le GetUserById a réussi";

            if (user == null)
            {
                logDescription = "Le GetUserById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 3, 6, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 3, 6, logDescription);
            return Ok(user);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] UpdateModel user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            string logDescription = "Le UpdateUserById a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le UpdateUserById a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 4, 6, logDescription);
                return BadRequest(ModelState);
            }                

            var _user = await _userRepository.UpdateUserById(id, user);

            if (_user is User)
            {
                await _logService.CreateLog(HttpContext, 4, 6, logDescription);
                var _userList = _context.Users;
                return Ok(_userList);
            }
            else
            {
                logDescription = "Le UpdateUserById a échoué";
                await _logService.CreateLog(HttpContext, 4, 6, logDescription);
                return BadRequest(_user);
            }
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            string logDescription = "Le DeleteUserById a réussi";
            var user = await _userRepository.DeleteUserById(id);
            
            if (user == null)
            {
                logDescription = "Le DeleteUserById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 5, 6, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 5, 6, logDescription);
            var _userList = _context.Users;
            return Ok(_userList);
        }

        /*
        [HttpGet]
        [Route("/secure/article-details")]
        public async Task<ActionResult<List<User>>> GetAllUserArticles()
        {
            return Ok();
        }*/
    }
}