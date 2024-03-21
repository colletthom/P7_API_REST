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
        private readonly LogService _logService;

        public TradeController(LocalDbContext context, TradeService tradeRepository, LogService logService)
        {
            _context = context;
            _tradeRepository = tradeRepository;
            _logService = logService;
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
            string logDescription = "Le AddTrade a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le AddTrade a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 1, 5, logDescription);
                return BadRequest(ModelState);
            }
            var addTrade = await _tradeRepository.AddTrade(trade);
            await _logService.CreateLog(HttpContext, 1, 5, logDescription);
            return Ok(addTrade);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var _trade = await _context.Trades.ToListAsync();
            string logDescription = "Le GetAll a réussi";

            await _logService.CreateLog(HttpContext, 2, 5, logDescription);
            return Ok(_trade);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            var _trade = await _context.Trades.FindAsync(id);
            string logDescription = "Le GetTradeById a réussi";
            if (_trade == null)
            {
                logDescription = "Le GetTradeById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 3, 5, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 3, 5, logDescription);
            return Ok(_trade);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            string logDescription = "Le UpdateTradeById a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le UpdateTradeById a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 4, 5, logDescription);
                return BadRequest(ModelState);
            }
                

            var _trade = await _tradeRepository.UpdateTradeById(id, trade);
            if (!_trade)
            {
                logDescription = "Le UpdateTradeById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 4, 5, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 4, 5, logDescription);
            var _tradeList = _context.Trades;
            return Ok(_tradeList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find Trade by Id and delete the Trade, return to Trade list
            string logDescription = "Le DeleteTradeById a réussi";
            var _trade = await _tradeRepository.DeleteTradeById(id);
            if (!_trade)
            {
                logDescription = "Le DeleteTradeById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 5, 5, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 5, 5, logDescription);
            var _tradeList = _context.Trades;
            return Ok(_tradeList);
        }
    }
}