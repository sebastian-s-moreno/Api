using Forte.Weather.Services.Models;

namespace Forte.Weather.Services
{
    public interface ILocationService
    {
        public List<Location> GetLocations();

        public Location? GetLocation(string id);

        public Task<bool> AddLocation(Location location);

        public bool DeleteLocation(string id);

        public Task<bool> UpdateLocation(string id, Location location);

        public Task<Models.Weather?> GetUpdatedDetails(string id);

        public Task<Models.Weather?> GetUpdatedDetails(double? longitude, double? latitude);
    }
}
