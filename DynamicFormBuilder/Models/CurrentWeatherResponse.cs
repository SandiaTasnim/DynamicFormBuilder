namespace DynamicFormBuilder.Models
{
    public class CurrentWeatherResponse
    {
        
        public Location Location { get; set; }
        public Current Current { get; set; }
    }

    public class ForecastWeatherResponse
    {
        public Location Location { get; set; }
        public Current Current { get; set; }
        public Forecast Forecast { get; set; }
    }

    public class HistoricalWeatherResponse
    {
        public Location Location { get; set; }
        public Forecast Forecast { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Localtime { get; set; }
    }

    public class Current
    {
        public decimal Temp_c { get; set; }
        public decimal Temp_f { get; set; }
        public Condition Condition { get; set; }
        public decimal Wind_kph { get; set; }
        public int Humidity { get; set; }
        public decimal Feelslike_c { get; set; }
        public decimal Feelslike_f { get; set; }
    }

    public class Forecast
    {
        public List<ForecastDay> Forecastday { get; set; }
    }

    public class ForecastDay
    {
        public string Date { get; set; }
        public Day Day { get; set; }
        public List<Hour> Hour { get; set; }
    }

    public class Day
    {
        public decimal Maxtemp_c { get; set; }
        public decimal Mintemp_c { get; set; }
        public decimal Avgtemp_c { get; set; }
        public decimal Maxtemp_f { get; set; }
        public decimal Mintemp_f { get; set; }
        public decimal Avgtemp_f { get; set; }
        public Condition Condition { get; set; }
        public int Daily_chance_of_rain { get; set; }
    }

    public class Hour
    {
        public string Time { get; set; }
        public decimal Temp_c { get; set; }
        public decimal Temp_f { get; set; }
        public Condition Condition { get; set; }
        public int Humidity { get; set; }
    }

    public class Condition
    {
        public string Text { get; set; }
        public string Icon { get; set; }
    }

    public class WeatherViewModel
    {
        public string City { get; set; }
        public CurrentWeatherResponse CurrentWeather { get; set; }
        public ForecastWeatherResponse ForecastWeather { get; set; }
        public HistoricalWeatherResponse HistoricalWeather { get; set; }
    }
}
