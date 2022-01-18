using Forte.Weather.DataAccess.Schema;

namespace Forte.Weather.DataAccess.Repository
{
    public interface ILocationRepository
    {

        public Task<List<LocationEntity>> GetLocations();

        public LocationEntity? GetLocation(string id);

        public void AddLocation(LocationEntity location);

        public void DeleteLocation(string id);

        public void UpdateLocation(string id, LocationEntity location);
    }
}
