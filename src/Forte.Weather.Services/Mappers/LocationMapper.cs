using Forte.Weather.DataAccess.Schema;
using Newtonsoft.Json;

namespace Forte.Weather.Services.Mappers
{
    public static class LocationMapper
    {
        public static List<LocationModel> ToModel(this List<LocationEntity> entities)
        {
            return entities.Select(entity => entity.ToModel()).ToList();
        }

        public static LocationModel ToModel(this LocationEntity entity)
        {
            return new()
            {
                ID = entity.ID,
                Name = entity.Name,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Timeserie = JsonConvert.DeserializeObject<TimeSerie>(entity.Timeserie)
            };
        }

        public static LocationEntity FromModel(this LocationModel entity)
        {
            return new()
            {
                ID = entity.ID,
                Name = entity.Name,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Timeserie = JsonConvert.SerializeObject(entity.Timeserie)
            };
        }
    }
}
