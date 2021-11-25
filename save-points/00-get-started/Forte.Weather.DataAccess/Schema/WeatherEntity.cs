using System.ComponentModel.DataAnnotations;

namespace Forte.Weather.DataAccess.Schema
{
    public class WeatherEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public int Temperature { get; set; }
    }
}