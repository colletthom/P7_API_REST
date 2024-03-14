using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public TradeController(LocalDbContext context)
        {
            _context = context;
        }
        // TODO: Inject Trade service
        /*
        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Trade, add to model
            return Ok();
        }

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody] Trade trade)
        {
            // TODO: check data valid and save to db, after saving return Trade list
            return Ok();
        }*/

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _trade = new Trade
            {
                Account = trade.Account,
                AccountType = trade.AccountType, 
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                CreationName = trade.CreationName,
                CreationDate = trade.CreationDate,
                RevisionName = trade.RevisionName,
                RevisionDate = trade.RevisionDate,
                DealName = trade.DealName,
                DealType = trade.DealType,
                SourceListId = trade.SourceListId,
                Side = trade.Side,
            };
            _context.Trades.Add(_trade);
            await _context.SaveChangesAsync();
            return Ok(_trade);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            var _trade = await _context.Trades.FindAsync(id);
            if (_trade == null)
                return NotFound();
            return Ok(_trade);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            var _trade = _context.Trades.Find(id);
            if (_trade == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _trade.Account = trade.Account;
            _trade.AccountType = trade.AccountType;
            _trade.SellQuantity = trade.SellQuantity;
            _trade.BuyPrice = trade.BuyPrice;
            _trade.SellPrice = trade.SellPrice;
            _trade.TradeDate = trade.TradeDate;
            _trade.TradeSecurity = trade.TradeSecurity;
            _trade.TradeStatus = trade.TradeStatus;
            _trade.Trader = trade.Trader;
            _trade.Benchmark = trade.Benchmark;
            _trade.Book = trade.Book;
            _trade.CreationName = trade.CreationName;
            _trade.CreationDate = trade.CreationDate;
            _trade.RevisionName = trade.RevisionName;
            _trade.RevisionDate = trade.RevisionDate;
            _trade.DealName = trade.DealName;
            _trade.DealType = trade.DealType;
            _trade.SourceListId = trade.SourceListId;
            _trade.Side = trade.Side;

            await _context.SaveChangesAsync();

            var _tradeList = _context.Trades;
            return Ok(_tradeList);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find Trade by Id and delete the Trade, return to Trade list
            var _trade = _context.Trades.Find(id);
            if (_trade == null)
                return NotFound();

            _context.Trades.Remove(_trade);
            await _context.SaveChangesAsync();

            var _tradeList = _context.Trades;
            return Ok(_tradeList);
        }
    }
}