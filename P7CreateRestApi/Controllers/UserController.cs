using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
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

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
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

        [HttpGet]
        //[Route("add")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]User user)
        {
            /* if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }
             var _user = new User
             {
                 UserName = user.UserName, 
                 Password = user.Password,
                 Fullname = user.Fullname,
                 Role = user.Role
             };
             _context.Users.Add(_user);
             await _context.SaveChangesAsync();
             return Ok(_trade);*/
            return Ok();
        }



        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            User user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);

            return Ok();
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser(int id)
        {
            User user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);

            return Ok();
        }

        [HttpGet]
        [Route("/secure/article-details")]
        public async Task<ActionResult<List<User>>> GetAllUserArticles()
        {
            return Ok();
        }
    }
}