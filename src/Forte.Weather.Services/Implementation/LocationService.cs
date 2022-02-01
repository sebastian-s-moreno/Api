using Forte.Weather.DataAccess.Repository;
using Forte.Weather.Services.Mappers;
using Forte.Weather.Services.Models;
using Yr.Facade;

namespace Forte.Weather.Services.Implementation
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IYrFacade _yrFacade;

        public LocationService(ILocationRepository repository, IYrFacade yrFacade)
        {
            _repository = repository;
            _yrFacade = yrFacade;
        }


        public List<Location> GetLocations()
        {
            try
            {
                var locations = _repository.GetLocations();
                return locations.ToModel();
            }
            catch
            {
                return new List<Location>();
            }
        }

        public async Task<bool> AddLocation(Location location)
        {
            try
            {
                location.Id = Guid.NewGuid().ToString();
                var ts = await GetUpdatedDetails(location.Longitude, location.Latitude);
                location.WeatherData = ts;
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

        public async Task<bool> UpdateLocation(string id, Location location)
        {
            try
            {
                var ts = await GetUpdatedDetails(id);
                location.WeatherData = ts;
                _repository.UpdateLocation(id, location.FromModel());
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Location? GetLocation(string id)
        {
            try
            {
                return _repository.GetLocation(id)?.ToModel();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Models.Weather?> GetUpdatedDetails(string id)
        {
            var location = GetLocation(id);

            return await GetUpdatedDetails(location?.Longitude, location?.Latitude);
        }

        public async Task<Models.Weather?> GetUpdatedDetails(double? longitude, double? latitude)
        {
            string elements;
            if (longitude != null && latitude != null)
            {
                elements = $"lat={latitude}&lon={longitude}";
            }
            else
            {
                return null;
            }

            var yrResponse = await _yrFacade.GetYrResponse(elements);
            return yrResponse.ToDomain();
        }
    }
}
