using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidListController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public BidListController(LocalDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _bidList = new BidList
            {
                Account = bidList.Account,
                BidType = bidList.BidType,
                BidQuantity = bidList.BidQuantity,
                AskQuantity = bidList.AskQuantity,
                Bid = bidList.Bid,
                Ask = bidList.Ask,
                Benchmark = bidList.Benchmark,
                BidListDate = bidList.BidListDate,
                Commentary = bidList.Commentary,
                BidSecurity = bidList.BidSecurity,
                BidStatus = bidList.BidStatus,
                Trader = bidList.Trader,
                Book = bidList.Book,
                CreationName = bidList.CreationName,
                CreationDate = bidList.CreationDate,
                RevisionName = bidList.RevisionName,
                RevisionDate = bidList.RevisionDate,
                DealName = bidList.DealName,
                DealType = bidList.DealType,
                SourceListId = bidList.SourceListId,
                Side = bidList.Side
            };
            _context.BidLists.Add(_bidList);
            await _context.SaveChangesAsync();
            return Ok(_bidList);
        }

        [HttpGet]
        //[Route("update/{id}")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var _bidList = await _context.BidLists.FindAsync(id);
            if (_bidList == null)
                return NotFound();
            return Ok(_bidList);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            var _bidList = _context.BidLists.Find(id);
            if (_bidList == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _bidList.Account = bidList.Account;
            _bidList.BidType = bidList.BidType;
            _bidList.BidQuantity = bidList.BidQuantity;
            _bidList.AskQuantity = bidList.AskQuantity;
            _bidList.Bid = bidList.Bid;
            _bidList.Ask = bidList.Ask;
            _bidList.Benchmark = bidList.Benchmark;
            _bidList.BidListDate = bidList.BidListDate;
            _bidList.Commentary = bidList.Commentary;
            _bidList.BidSecurity = bidList.BidSecurity;
            _bidList.BidStatus = bidList.BidStatus;
            _bidList.Trader = bidList.Trader;
            _bidList.Book = bidList.Book;
            _bidList.CreationName = bidList.CreationName;
            _bidList.CreationDate = bidList.CreationDate;
            _bidList.RevisionName = bidList.RevisionName;
            _bidList.RevisionDate = bidList.RevisionDate;
            _bidList.DealName = bidList.DealName;
            _bidList.DealType = bidList.DealType;
            _bidList.SourceListId = bidList.SourceListId;
            _bidList.Side = bidList.Side;

            await _context.SaveChangesAsync();

            var listBid = _context.BidLists;
            return Ok(listBid);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById (int id)
        {
            var _bidList = _context.BidLists.Find(id);
            if (_bidList == null)
                return NotFound();

            _context.BidLists.Remove(_bidList);
            await _context.SaveChangesAsync();

            var listBid = _context.BidLists;
            return Ok(listBid);
        }
    }
}