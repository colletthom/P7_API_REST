using Microsoft.Extensions.Configuration;
using System.Data.SqlTypes;

namespace Dot.Net.WebApi.Controllers
{
    public static class Logger
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static void WriteLog(string message)
        {
            string logPath = _configuration["logging:logPath"];
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine($"{ DateTime.Now} : {message} ");
            }
        }
    }
}
