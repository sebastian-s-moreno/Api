using Forte.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;

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
        public Location? GetRecommended(string preference)
        {
            Location location = null;
            if (Locations.Count() > 0)
            {
                switch (preference)
                {
                    case "Swimming":
                        location = Locations.OrderByDescending(x => x.Timeserie?.Data.Instant.Details.Air_temperature).First();
                        break;
                    case "Sailing":
                        location = Locations.OrderByDescending(x => x.Timeserie?.Data.Instant.Details.Wind_speed).First();
                        break;
                    case "Skiing":
                        location = Locations.OrderBy(x => x.Timeserie?.Data.Instant.Details.Air_temperature).First();
                        break;
                    case "Sightseeing":
                        location = Locations.OrderBy(x => x.Timeserie?.Data.Instant.Details.Air_pressure_at_sea_level).First();
                        break;
                    case "Unspecified":
                        var index = new Random().Next(Locations.Count);
                        location = Locations[index];
                        break;

                }
                return location;

            }
            else
            {
                return null; //Returner feilmelding og si at man må legge til locations
            }
        }

        [HttpGet("locations")]
        public IEnumerable<Location> Get()
        {
            return Locations;
        }

        [HttpPost("locations")]
        public async Task<OkResult> Post([FromBody] Location location)
        {
            location.ID = Guid.NewGuid().ToString();
            double latitude = double.Parse(location.Latitude, CultureInfo.InvariantCulture);
            double longitude = double.Parse(location.Longitude, CultureInfo.InvariantCulture);
            TimeSerie? ts = await GetDetails(latitude, longitude);
            location.Timeserie = ts;
            Locations.Add(location);
            return Ok();
        }

        [HttpPost("locations/delete")]
        public OkResult Delete([FromBody] string id)
        {
            Locations.Remove(Locations.Single(s => s.ID == id));
            return Ok();
        }

        [HttpGet("locations/details")]
        public async Task<TimeSerie?> GetDetails(double lat, double lon)
        {
            var elements = $"lat={lat}&lon={lon}";
            var url = "https://api.met.no/weatherapi/locationforecast/2.0/compact?" + elements;
            YrApiResponse? response = null;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var productValue = new ProductInfoHeaderValue("ForteWeatherApp", "1.0");
                request.Headers.UserAgent.Add(productValue);
                var httpResponse = await client.SendAsync(request);
                response = await httpResponse.Content.ReadFromJsonAsync<YrApiResponse>();
            }
            return response?.Properties?.Timeseries?.FirstOrDefault();
        }


        public class Location
        {
            public string? ID { get; set; }
            public string Name { get; set; } = "";
            public string Latitude { get; set; } = "";
            public string Longitude { get; set; } = "";
            public TimeSerie? Timeserie { get; set; }
        }

        public class YrApiResponse
        {
            public string? Type { get; set; }
            public Property Properties { get; set; } = new();
        }
        public class Property
        {
            public List<TimeSerie> Timeseries { get; set; } = new();
        }
        public class TimeSerie
        {
            public Data Data { get; set; } = new();
            public DateTimeOffset Time { get; set; }
        }
        public class Data
        {
            public Instant Instant { get; set; } = new();
        }
        public class Instant
        {
            public Details Details { get; set; } = new();
        }
        public class Details
        {
            public double Air_pressure_at_sea_level { get; set; }
            public double Air_temperature { get; set; }
            public double Relative_humidity { get; set; }
            public double Wind_from_direction { get; set; }
            public double Wind_speed { get; set; }
        }
    }
}