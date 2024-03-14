using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public BidController(LocalDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Bid bid)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _bid = new Bid
            {
                Account = bid.Account,
                BidType = bid.BidType,
                BidQuantity = bid.BidQuantity,
                AskQuantity = bid.AskQuantity,
                Bid2 = bid.Bid2,
                Ask = bid.Ask,
                Benchmark = bid.Benchmark,
                BidListDate = bid.BidListDate,
                Commentary = bid.Commentary,
                BidSecurity = bid.BidSecurity,
                BidStatus = bid.BidStatus,
                Trader = bid.Trader,
                Book = bid.Book,
                CreationName = bid.CreationName,
                CreationDate = bid.CreationDate,
                RevisionName = bid.RevisionName,
                RevisionDate = bid.RevisionDate,
                DealName = bid.DealName,
                DealType = bid.DealType,
                SourceListId = bid.SourceListId,
                Side = bid.Side
            };
            _context.Bids.Add(_bid);
            await _context.SaveChangesAsync();
            return Ok(_bid);
        }

        [HttpGet]
        //[Route("update/{id}")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var _bid = await _context.Bids.FindAsync(id);
            if (_bid == null)
                return NotFound();
            return Ok(_bid);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Bid bid)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            var _bid = _context.Bids.Find(id);
            if (_bid == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _bid.Account = bid.Account;
            _bid.BidType = bid.BidType;
            _bid.BidQuantity = bid.BidQuantity;
            _bid.AskQuantity = bid.AskQuantity;
            _bid.Bid2 = bid.Bid2;
            _bid.Ask = bid.Ask;
            _bid.Benchmark = bid.Benchmark;
            _bid.BidListDate = bid.BidListDate;
            _bid.Commentary = bid.Commentary;
            _bid.BidSecurity = bid.BidSecurity;
            _bid.BidStatus = bid.BidStatus;
            _bid.Trader = bid.Trader;
            _bid.Book = bid.Book;
            _bid.CreationName = bid.CreationName;
            _bid.CreationDate = bid.CreationDate;
            _bid.RevisionName = bid.RevisionName;
            _bid.RevisionDate = bid.RevisionDate;
            _bid.DealName = bid.DealName;
            _bid.DealType = bid.DealType;
            _bid.SourceListId = bid.SourceListId;
            _bid.Side = bid.Side;

            await _context.SaveChangesAsync();

            var bidList = _context.Bids;
            return Ok(bidList);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById (int id)
        {
            var _bid = _context.Bids.Find(id);
            if (_bid == null)
                return NotFound();

            _context.Bids.Remove(_bid);
            await _context.SaveChangesAsync();

            var bidList = _context.Bids;
            return Ok(bidList);
        }
    }
}