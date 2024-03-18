using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly BidRepository _bidRepository;
        private readonly LocalDbContext _context;
        //private readonly ILogger<BidController> _logger;

        public BidController(BidRepository bidRepository, LocalDbContext context)
        //public BidController(BidRepository bidRepository, LocalDbContext context, ILogger<BidController> logger)
        {
            _bidRepository = bidRepository;
            _context = context;
            //_logger = logger;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Bid bid)
        {
            // TODO: check data valid and save to db, after saving return bid list
            //_logger.LogInformation("this is AddBid");
            //Logger.WriteLog("this is AddBid");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedBid = await _bidRepository.AddBid(bid);
            return Ok(addedBid);

        }

        [HttpGet]
        //[Route("update/{id}")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var _bid = await _bidRepository.GetBidById(id);
            if (_bid == null)
                return NotFound();
            return Ok(_bid);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Bid bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateBid = await _bidRepository.UpdateBid(id, bid);
            if (!updateBid)
                return NotFound();

            var bidList = _context.Bids;
             return Ok(bidList);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById (int id)
        {
            var _bid = await _bidRepository.DeleteBid(id);
            if (!_bid)
                return NotFound();

            var bidList = _context.Bids;
            return Ok(bidList);
        }
    }
}