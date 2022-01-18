using Forte.Weather.DataAccess.Repository;
using Forte.Weather.Services.Mappers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Forte.Weather.Services
{
    public class LocationService : ILocationService 
    {
        private readonly ILocationRepository _repository;

        public LocationService(ILocationRepository repository)
        {
            _repository = repository;
        }


        public LocationModel? GetLocation(string id)
        {
            return _repository.GetLocation(id)?.ToModel();
        }

        public async Task<bool> AddLocation(LocationModel location)
        {
            try
            {
                location.ID = Guid.NewGuid().ToString();
                TimeSerie? ts = await GetUpdatedDetails(null,location.Longitude.ToString(),location.Latitude.ToString());
                location.Timeserie = ts;
                _repository.AddLocation(location.FromModel());
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteLocation(string id)
        {
            try
            {
                _repository.DeleteLocation(id);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateLocation(string id, LocationModel location)
        {
            try
            {
                TimeSerie? ts = await GetUpdatedDetails(id,null,null);
                location.Timeserie = ts;
                _repository.UpdateLocation(id,location.FromModel());
            }
            catch
            {
                return false;
            }
            return true;
        }



        public async Task<List<LocationModel>> GetLocations()
        {
            var locations = await _repository.GetLocations();
            return locations.ToModel();
        }

        public async Task<LocationModel?> GetRecommendedLocation(string activity)
        {
            List<LocationModel> Locations = await GetLocations();

            LocationModel? location = null;
            if (Locations.Count() > 0)
            {
                switch (activity)
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
                return null;
            }
        }

        public async Task<TimeSerie?> GetUpdatedDetails(string? id, string? longitude, string? latitude)
        {
            string elements = "";
            if (id != null)
            {
                LocationModel? location = GetLocation(id);
                if (location != null){
                    elements = $"lat={location.Latitude}&lon={location.Longitude}";
                }
            }
            else
            {
                elements = $"lat={latitude}&lon={longitude}";
            }
            if (elements != "")
            {
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
            else
            {
                return null;
            }
        }
            

    }
}
