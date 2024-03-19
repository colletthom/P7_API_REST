using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserRepository _userRepository;
        private readonly LocalDbContext _context;

        public UserController(UserRepository userRepository, LocalDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
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
        [Authorize(Policy = "AccessWriteActions")]
        //[Route("add")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]User user)
        {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

            var addUser = await _userRepository.AddUser(user);
            return Ok(addUser); 
        }

        [HttpGet]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] User user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _user = await _userRepository.UpdateUserById(id, user);
            if (!_user)
                return NotFound();

            var _userList = _context.Users;
            return Ok(_userList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.DeleteUserById(id);
            
            if (user == null)
                return NotFound();

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