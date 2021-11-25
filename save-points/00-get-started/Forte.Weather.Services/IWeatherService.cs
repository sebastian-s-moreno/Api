namespace Forte.Weather.Services
{
    public interface IWeatherService
    {
        public Task<IEnumerable<WeatherModel>> WeatherForcastAsync(int days);
    }
}
