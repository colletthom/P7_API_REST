using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Repositories
{ 
    public class RuleNameRepository
    {
        private LocalDbContext _context { get; }

        public RuleNameRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<RuleName> AddRuleName(RuleName ruleName)
        {
            var _ruleName = new RuleName
            {
                Name = ruleName.Name,
                Description = ruleName.Description,
                Json = ruleName.Json,
                Template = ruleName.Template,
                SqlStr = ruleName.SqlStr,
                SqlPart = ruleName.SqlPart,
            };
            _context.RuleNames.Add(_ruleName);
            await _context.SaveChangesAsync();
            return _ruleName;
        }

        public async Task<RuleName> GetRuleNameById(int id)
        {
            return await _context.RuleNames.FindAsync(id);
        }

        public async Task<bool> UpdateRuleNameById(int id, RuleName ruleName)
        {
            var _ruleName = _context.RuleNames.Find(id);
            if (_ruleName == null)
            {
                return false;
            }

            _ruleName.Name = ruleName.Name;
            _ruleName.Description = ruleName.Description;
            _ruleName.Json = ruleName.Json;
            _ruleName.Template = ruleName.Template;
            _ruleName.SqlStr = ruleName.SqlStr;
            _ruleName.SqlPart = ruleName.SqlPart;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRuleNameById(int id)
        {
            var ruleName = await _context.RuleNames.FindAsync(id);
            if (ruleName == null)
            {
                return false; // Or throw an exception
            }

            _context.RuleNames.Remove(ruleName);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
