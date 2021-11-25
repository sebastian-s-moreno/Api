using Forte.Weather.DataAccess.Repository;
using Forte.Weather.Services.Mappers;

namespace Forte.Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _repository;

        public WeatherService(IWeatherRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WeatherModel>> WeatherForcastAsync(int days)
        {
            return (await _repository.GetForecast(days)).ToModel();
        }
    }
}
