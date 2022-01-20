 using System.ComponentModel.DataAnnotations;

namespace Forte.Weather.Services
{

    public class LocationModel
    {
        public string? ID { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Range(-90,90)]
        public double Latitude { get; set; }
        [Range(-180,80)]
        public double Longitude { get; set; }
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