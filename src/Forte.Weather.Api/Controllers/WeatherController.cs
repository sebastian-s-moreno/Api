using Forte.Weather.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forte.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public static List<Location> Locations = new List<Location>();

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("locations/recommended")]
        public Location GetRecommended()
        {
            var index = new Random().Next(Locations.Count);

            return Locations[index];
        }

        [HttpGet("locations")]
        public IEnumerable<Location> Get()
        {
            return Locations;
        }

        [HttpPost("locations")]
        public OkResult Post([FromBody] Location location)
        {
            Locations.Add(location);
            return Ok();
        }

        public class Location
        {
            public string Name { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }
    }
}