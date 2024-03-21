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

        public RatingController(LocalDbContext context, RatingService ratingRepository)
        {
            _context = context;
            _ratingRepository = ratingRepository;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addRating = await _ratingRepository.AddRating(rating);
            return Ok(addRating);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            var _rating = await _ratingRepository.GetRatingById(id);
            if (_rating == null)
                return NotFound();
            return Ok(_rating);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _rating = await _ratingRepository.UpdateRatingById(id, rating);
            if (!_rating)
                return NotFound();

            var _ratinglist = _context.Ratings;
            return Ok(_ratinglist);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            var _rating = await _ratingRepository.DeleteRatingById(id);
            if (!_rating)
                return NotFound();

            var _ratingList = _context.Ratings;
            return Ok(_ratingList);
        }
    }
}