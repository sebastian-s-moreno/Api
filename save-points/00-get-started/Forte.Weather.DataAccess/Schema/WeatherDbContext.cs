using Forte.Weather.DataAccess.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Forte.Weather.DataAccess.Schema
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
            if (Database.IsSqlite())
            {
                Database.EnsureCreated();
            }
        }

        public DbSet<WeatherEntity> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
