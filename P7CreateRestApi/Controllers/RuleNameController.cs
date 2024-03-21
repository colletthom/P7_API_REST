using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuleNameController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly RuleNameService _ruleNameRepository;
        private readonly LogService _logService;

        public RuleNameController(LocalDbContext context, RuleNameService ruleNameRepository, LogService logService)
        {
            _context = context;
            _ruleNameRepository = ruleNameRepository;
            _logService = logService;
        }
        // TODO: Inject RuleName service
        /*
        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all RuleName, add to model
            return Ok();
        }

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody] RuleName trade)
        {
            // TODO: check data valid and save to db, after saving return RuleName list
            return Ok();
        }*/

        [HttpPost]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]RuleName ruleName)
        {
            string logDescription = "Le AddRuleName a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le AddRuleNAme a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 1, 4, logDescription);
                return BadRequest(ModelState);
            }
            var addRuleName = await _ruleNameRepository.AddRuleName(ruleName);
            await _logService.CreateLog(HttpContext, 1, 4, logDescription);
            return Ok(addRuleName);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var _ruleName = await _context.Bids.ToListAsync();
            string logDescription = "Le GetAll a réussi";

            await _logService.CreateLog(HttpContext, 2, 4, logDescription);
            return Ok(_ruleName);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            var _ruleName = await _context.RuleNames.FindAsync(id);
            string logDescription = "Le GetRuleNameById a réussi";
            if (_ruleName == null)
            {
                logDescription = "Le GetRuleNameById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 3, 4, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 3, 4, logDescription);
            return Ok(_ruleName);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] RuleName ruleName)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            string logDescription = "Le UpdateRuleNameById a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le UpdateRuleNameById a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 4, 4, logDescription);
                return BadRequest(ModelState);
            }              

            var _ruleName = await _ruleNameRepository.UpdateRuleNameById(id, ruleName);
            if (!_ruleName)
            {
                logDescription = "Le UpdateRuleNameById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 4, 4, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 4, 4, logDescription);
            var _ruleNameList = _context.RuleNames;
            return Ok(_ruleNameList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            string logDescription = "Le DeleteRuleNameById a réussi";
            var _ruleName = await _ruleNameRepository.DeleteRuleNameById(id);
            if (!_ruleName)
            {
                logDescription = "Le DeleteRuleNameById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 5, 4, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 5, 4, logDescription);
            var _ruleNameList = _context.RuleNames;
            return Ok(_ruleNameList);
        }
    }
}