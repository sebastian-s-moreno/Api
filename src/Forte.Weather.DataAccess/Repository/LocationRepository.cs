using Forte.Weather.DataAccess.Schema;
using Microsoft.EntityFrameworkCore;

namespace Forte.Weather.DataAccess.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationDbContext _context;

        public LocationRepository(LocationDbContext context)
        {
            _context = context;
        }

        public void AddLocation(LocationEntity location)
        {
            _context.Locations.Add(location);
            _context.SaveChanges();
        }

        public void DeleteLocation(string id)
        {
            var entry = _context.Locations.FirstOrDefault(entity => entity.ID == id);
            if (entry != null)
            {
                _context.Locations.Remove(entry);
                _context.SaveChanges();
            }
        }
        public LocationEntity? GetLocation(string id)
        {
            var entry = _context.Locations.FirstOrDefault(entity => entity.ID == id);
            return entry;
        }

        public async Task<List<LocationEntity>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        public void UpdateLocation(string id, LocationEntity location)
        {
            var entry = _context.Locations.FirstOrDefault(entity => entity.ID == id);
            _context.Entry(entry).CurrentValues.SetValues(location);
            _context.SaveChanges();
        }
    }
}
