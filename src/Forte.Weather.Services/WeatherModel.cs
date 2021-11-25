namespace Forte.Weather.Services
{
    public class WeatherModel
    {
        public DateTime Date { get; set; }

        public int Celcius { get; set; }

        public int Farenheit => 32 + (int)(Celcius / 0.5556);

        public string? Summary { get; set; }
    }
}