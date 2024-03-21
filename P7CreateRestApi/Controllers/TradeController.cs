using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        private readonly TradeService _tradeRepository;

        public TradeController(LocalDbContext context, TradeService tradeRepository)
        {
            _context = context;
            _tradeRepository = tradeRepository;
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
        [Authorize(Policy = "AccessWriteActions")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addTrade = await _tradeRepository.AddTrade(trade);
            return Ok(addTrade);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            var _trade = await _tradeRepository.GetTradeById(id);
            if (_trade == null)
                return NotFound();
            return Ok(_trade);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _trade = await _tradeRepository.UpdateTradeById(id, trade);
            if (!_trade)
                return NotFound();

            var _tradeList = _context.Trades;
            return Ok(_tradeList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find Trade by Id and delete the Trade, return to Trade list
            var _trade = await _tradeRepository.DeleteTradeById(id);
            if (!_trade)
                return NotFound();

            var _tradeList = _context.Trades;
            return Ok(_tradeList);
        }
    }
}