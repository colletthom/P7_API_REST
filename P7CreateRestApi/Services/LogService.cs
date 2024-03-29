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

        public async Task<bool> CreateLog(HttpContext _httpContext, int crud, int entity, string logDescription)
        {
            int userId=0;
            string userName = null;
            var userIdClaim = _httpContext.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out int parseUserId))
                    userId = parseUserId;
            }
            else
            {
                string[] parts = logDescription.Split(new string[] { "Description: " }, 2, StringSplitOptions.None);

                userName = parts[0];
                logDescription = parts[1];
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
                userId = user.Id;
            }
            

            Log myLog = new Log
            {
                UserID = userId,
                LogDateTime= DateTime.UtcNow,
                CRUD = crud,
                Entity = entity,
                LogDescription = logDescription,
            };
            _context.Logs.Add(myLog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
