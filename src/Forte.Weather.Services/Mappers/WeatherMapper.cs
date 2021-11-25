using Forte.Weather.DataAccess.Schema;

namespace Forte.Weather.Services.Mappers
{
    public static class WeatherMapper
    {
        public static IEnumerable<WeatherModel> ToModel(this IEnumerable<WeatherEntity> entities)
        {
            return entities.Select(entity => entity.ToModel()).ToList();
        }

        public static WeatherModel ToModel(this WeatherEntity entity)
        {
            return new()
            {
                Date = entity.Date,
                Celcius = entity.Temperature,
                Summary = entity.Summary,
            };
        }
    }
}
