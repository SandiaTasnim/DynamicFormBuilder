using DynamicFormBuilder.Models;

namespace DynamicFormBuilder.ViewModels
{
    public class WeatherViewModel
    {
        public CurrentWeatherDto Current { get; set; }
        public List<DailyForecastDto> Forecast { get; set; }
    }
}
