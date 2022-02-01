using Forte.Weather.DataAccess.Schema;
using Forte.Weather.Services.Models;

namespace Forte.Weather.Services.Mappers
{
    public static class LocationMapper
    {
        public static List<Location> ToModel(this List<LocationEntity> entities)
        {
            return entities.Select(entity => entity.ToModel()).ToList();
        }

        public static Location ToModel(this LocationEntity entity)
        {
            return new Location
            {
                Id = entity.ID,
                Name = entity.Name,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                WeatherData = new Models.Weather
                {
                    AirPressureAtSeaLevel = entity.AirPressureAtSeaLevel,
                    AirTemperature = entity.AirTemperature,
                    RelativeHumidity = entity.RelativeHumidity,
                    WindFromDirection = entity.WindFromDirection,
                    WindSpeed = entity.WindSpeed
                }
            };
        }

        public static LocationEntity FromModel(this Location model)
        {
            return new LocationEntity
            {
                ID = model.Id,
                Name = model.Name,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                AirPressureAtSeaLevel = model.WeatherData?.AirPressureAtSeaLevel,
                AirTemperature = model.WeatherData?.AirTemperature,
                RelativeHumidity = model.WeatherData?.RelativeHumidity,
                WindFromDirection = model.WeatherData?.WindFromDirection,
                WindSpeed = model.WeatherData?.WindSpeed
            };
        }
    }
}
