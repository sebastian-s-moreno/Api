using Forte.Weather.DataAccess.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Forte.Weather.DataAccess.Schema
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options)
            : base(options)
        {
            if (Database.IsSqlite())
            {
                Database.EnsureCreated();
            }
        }

        public DbSet<LocationEntity> Locations { get; set; } 


    }
}
