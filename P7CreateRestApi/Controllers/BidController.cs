using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;*/

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly BidRepository _bidRepository;
        private readonly LocalDbContext _context;
        private readonly LogService _logService;

        public BidController(BidRepository bidRepository, LocalDbContext context, LogService logService)
        {
            _bidRepository = bidRepository;
            _context = context;
            _logService = logService;
        }

        [HttpPost]
        [Authorize(Policy = "AccessWriteActions")]       
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Bid bid)
        {
            // TODO: check data valid and save to db, after saving return bid list
            string logDescription = "Le AddBid a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le AddBid a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 1, 1, logDescription);
                return BadRequest(ModelState);
            }
            var addedBid = await _bidRepository.AddBid(bid);
            await _logService.CreateLog(HttpContext, 1, 1, logDescription);
            return Ok(addedBid);

        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        //[Route("update/{id}")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var _bid = await _bidRepository.GetBidById(id);
            string logDescription="Le GetBidById a réussi";
            if (_bid == null)
            {
                logDescription = "Le GetBidById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 2, 1, logDescription);
                return NotFound();
            }
                
            await _logService.CreateLog(HttpContext, 2, 1,logDescription);
            return Ok(_bid);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Bid bid)
        {
            string logDescription = "Le UpdateBidById a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le UpdateBidById a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 3, 1, logDescription);
                return BadRequest(ModelState);
            }

            var updateBid = await _bidRepository.UpdateBid(id, bid);
            if (!updateBid)
            {
                logDescription = "Le UpdateBidById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 3, 1, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 3, 1, logDescription);
            var bidList = _context.Bids;            
            return Ok(bidList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById (int id)
        {
            string logDescription = "Le DeleteBidById a réussi";
            var _bid = await _bidRepository.DeleteBid(id);
            if (!_bid)
            {
                logDescription = "Le DeleteBidById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 4, 1, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 4, 1, logDescription);
            var bidList = _context.Bids;
            return Ok(bidList);
        }
    }
}