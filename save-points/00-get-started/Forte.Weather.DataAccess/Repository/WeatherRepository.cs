using Forte.Weather.DataAccess.Schema;
using Microsoft.EntityFrameworkCore;

namespace Forte.Weather.DataAccess.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeatherEntity>> GetForecast(int quantity)
        {
            return await _context.Weather
                .OrderBy(entity => entity.Id)
                .Take(quantity)
                .ToListAsync();
        }
    }
}
