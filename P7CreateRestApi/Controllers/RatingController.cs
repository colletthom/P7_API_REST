using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly RatingService _ratingRepository;
        private readonly LogService _logService;

        public RatingController(LocalDbContext context, RatingService ratingRepository, LogService logService)
        {
            _context = context;
            _ratingRepository = ratingRepository;
            _logService = logService;
        }
        // TODO: Inject Rating service
        /*
        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Rating, add to model
            return Ok();
        }

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody] Rating rating)
        {
            // TODO: check data valid and save to db, after saving return Rating list
            return Ok();
        }*/

        [HttpPost]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]Rating rating)
        {
            string logDescription = "Le AddBid a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le AddRating a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 1, 3, logDescription);
                return BadRequest(ModelState);
            }
            var addRating = await _ratingRepository.AddRating(rating);
            await _logService.CreateLog(HttpContext, 1, 3, logDescription);
            return Ok(addRating);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var _rating = await _context.Ratings.ToListAsync();
            string logDescription = "Le GetAll a réussi";

            await _logService.CreateLog(HttpContext, 2, 3, logDescription);
            return Ok(_rating);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            string logDescription = "Le GetRatingById a réussi";
            var _rating = await _context.Ratings.FindAsync(id);
            if (_rating == null)
            {
                logDescription = "Le GetRatingById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 3, 3, logDescription);
                return NotFound();
            }
            await _logService.CreateLog(HttpContext, 3, 3, logDescription);
            return Ok(_rating);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            string logDescription = "Le UpdateRatingById a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le UpdateRatingById a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 4, 3, logDescription);
                return BadRequest(ModelState);
            }               

            var _rating = await _ratingRepository.UpdateRatingById(id, rating);
            if (!_rating)
            {
                logDescription = "Le UpdateRatingById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 4, 3, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 4, 3, logDescription);
            var _ratinglist = _context.Ratings;
            return Ok(_ratinglist);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            string logDescription = "Le DeleteRatingById a réussi";
            var _rating = await _ratingRepository.DeleteRatingById(id);
            if (!_rating)
            {
                logDescription = "Le DeleteRatingById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 5, 3, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 5, 3, logDescription);
            var _ratingList = _context.Ratings;
            return Ok(_ratingList);
        }
    }
}