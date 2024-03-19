using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuleNameController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly RuleNameRepository _ruleNameRepository;

        public RuleNameController(LocalDbContext context, RuleNameRepository ruleNameRepository)
        {
            _context = context;
            _ruleNameRepository = ruleNameRepository;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addRuleName = await _ruleNameRepository.AddRuleName(ruleName);
            return Ok(addRuleName);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction,AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            var _ruleName = await _ruleNameRepository.GetRuleNameById(id);
            if (_ruleName == null)
                return NotFound();
            return Ok(_ruleName);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] RuleName ruleName)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _ruleName = await _ruleNameRepository.UpdateRuleNameById(id, ruleName);
            if (!_ruleName)
                return NotFound();

            var _ruleNameList = _context.RuleNames;
            return Ok(_ruleNameList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            var _ruleName = await _ruleNameRepository.DeleteRuleNameById(id);
            if (!_ruleName)
                return NotFound();

            var _ruleNameList = _context.RuleNames;
            return Ok(_ruleNameList);
        }
    }
}