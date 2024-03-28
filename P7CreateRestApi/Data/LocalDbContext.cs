using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dot.Net.WebApi.Data
{
    public interface IDbContext
    {
        DbSet<Bid> Bids { get; set; }
        DbSet<User> Users { get; set; }

        DbSet<CurvePoint> CurvePoints { get; set; }

        DbSet<Rating> Ratings { get; set; }

        DbSet<RuleName> RuleNames { get; set; }

        DbSet<Trade> Trades { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    public class LocalDbContext : DbContext, IDbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set;}
        public DbSet<Bid> Bids { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}