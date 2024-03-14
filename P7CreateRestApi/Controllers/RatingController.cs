using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public RatingController(LocalDbContext context)
        {
            _context = context;
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
        [Route("")]
        public async Task<IActionResult> Add([FromBody]Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _rating = new Rating
            {

                MoodysRating = rating.MoodysRating,
                SandPRating = rating.SandPRating,
                FitchRating = rating.FitchRating, 
                OrderNumber = rating.OrderNumber 
            };
            _context.Ratings.Add(_rating);
            await _context.SaveChangesAsync();
            return Ok(_rating);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            var _rating = await _context.Ratings.FindAsync(id);
            if (_rating == null)
                return NotFound();
            return Ok(_rating);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            var _rating = _context.Ratings.Find(id);
            if (_rating == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _rating.MoodysRating = rating.MoodysRating;
            _rating.SandPRating = rating.SandPRating;
            _rating.FitchRating = rating.FitchRating;
            _rating.OrderNumber = rating.OrderNumber;

            await _context.SaveChangesAsync();

            var _ratinglist = _context.Ratings;
            return Ok(_ratinglist);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            var _rating = _context.Ratings.Find(id);
            if (_rating == null)
                return NotFound();

            _context.Ratings.Remove(_rating);
            await _context.SaveChangesAsync();

            var _ratingList = _context.Ratings;
            return Ok(_ratingList);
        }
    }
}