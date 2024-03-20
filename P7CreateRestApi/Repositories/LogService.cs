using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class LogService
    {
        private LocalDbContext _context { get; }

        public LogService(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateLog(HttpContext _httpContext, int crud, int entity)
        {
            int userId=0;
            var userIdClaim = _httpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out int parseUserId))
                    userId = parseUserId;
            }



            Log myLog = new Log
            {
                UserID = userId,
                LogDateTime= DateTime.UtcNow,
                CRUD = crud,
                Entity = entity,

            };
            _context.Logs.Add(myLog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
