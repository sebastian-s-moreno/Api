using Forte.Weather.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forte.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
                private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{days}")]
        public async Task<IEnumerable<WeatherModel>> Get(int days = 5)
        {
            return await _weatherService.WeatherForcastAsync(days);
        }
    }
}