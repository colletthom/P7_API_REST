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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("Admin")]
        public IActionResult Admin()
        {
            return Ok();
        }
    }
}