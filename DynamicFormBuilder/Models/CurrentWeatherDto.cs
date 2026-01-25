namespace DynamicFormBuilder.Models
{
    public class CurrentWeatherDto
    {
        public string Name { get; set; }
        public Main Main { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }
}
