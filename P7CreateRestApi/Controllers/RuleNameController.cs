using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuleNameController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public RuleNameController(LocalDbContext context)
        {
            _context = context;
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
        [Route("")]
        public async Task<IActionResult> Add([FromBody]RuleName ruleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _ruleName = new RuleName
            {
                Name =ruleName.Name,
                Description  = ruleName.Description,
                Json = ruleName.Json,
                Template = ruleName.Template,
                SqlStr = ruleName.SqlStr,
                SqlPart = ruleName.SqlPart,
            };
            _context.RuleNames.Add(_ruleName);
            await _context.SaveChangesAsync();
            return Ok(_ruleName);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            var _ruleName = await _context.RuleNames.FindAsync(id);
            if (_ruleName == null)
                return NotFound();
            return Ok(_ruleName);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] RuleName ruleName)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            var _ruleName = _context.RuleNames.Find(id);
            if (_ruleName == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _ruleName.Name = ruleName.Name;
            _ruleName.Description = ruleName.Description;
            _ruleName.Json = ruleName.Json;
            _ruleName.Template = ruleName.Template;
            _ruleName.SqlStr = ruleName.SqlStr;
            _ruleName.SqlPart = ruleName.SqlPart;

            await _context.SaveChangesAsync();

            var _ruleNameList = _context.RuleNames;
            return Ok(_ruleNameList);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            var _ruleName = _context.RuleNames.Find(id);
            if (_ruleName == null)
                return NotFound();

            _context.RuleNames.Remove(_ruleName);
            await _context.SaveChangesAsync();

            var _ruleNameList = _context.RuleNames;
            return Ok(_ruleNameList);
        }
    }
}