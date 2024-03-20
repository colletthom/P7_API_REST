using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly BidRepository _bidRepository;
        private readonly LocalDbContext _context;
        private readonly LogService _logService;

        //private readonly ILogger<BidController> _logger;

        public BidController(BidRepository bidRepository, LocalDbContext context, LogService logService)
        //public BidController(BidRepository bidRepository, LocalDbContext context, ILogger<BidController> logger)
        {
            _bidRepository = bidRepository;
            _context = context;
            _logService = logService;
            //_logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = "AccessWriteActions")]       
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
        [Authorize(Policy = "AccessGetAction")]
        //[Route("update/{id}")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var _bid = await _bidRepository.GetBidById(id);
            if (_bid == null)
                return NotFound();

            await _logService.CreateLog(HttpContext, 2, 1);

            return Ok(_bid);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
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
        [Authorize(Policy = "AccessWriteActions")]
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