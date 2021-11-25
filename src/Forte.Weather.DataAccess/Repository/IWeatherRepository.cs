using Forte.Weather.DataAccess.Schema;

namespace Forte.Weather.DataAccess.Repository
{
    public interface IWeatherRepository
    {
        public Task<IEnumerable<WeatherEntity>> GetForecast(int quantity);
    }
}
