using Forte.Weather.Services.Models;

namespace Forte.Weather.Services.Implementation
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ILocationService _locationService;

        public RecommendationService(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public Location? GetRecommendedLocation(Activity activity)
        {
            var locations = _locationService.GetLocations();

            if (locations.Any())
            {
                switch (activity)
                {
                    case Activity.Swimming:
                        return locations.OrderByDescending(x => x.WeatherData?.AirTemperature).First();
                    case Activity.Sailing:
                        return locations.OrderByDescending(x => x.WeatherData?.WindSpeed).First();
                    case Activity.Skiing:
                        return locations.OrderBy(x => x.WeatherData?.AirTemperature).First();
                    case Activity.Sightseeing:
                        return locations.OrderBy(x => x.WeatherData?.AirPressureAtSeaLevel).First();
                    case Activity.Unspecified:
                        var index = new Random().Next(locations.Count);
                        return locations[index];
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
