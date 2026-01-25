namespace DynamicFormBuilder.Services.Interfaces
{
    using DynamicFormBuilder.Models;

    public interface IWeatherService
    {
        Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city);
        Task<ForecastWeatherResponse> GetForecastWeatherAsync(string city, int days = 7);
        Task<HistoricalWeatherResponse> GetHistoricalWeatherAsync(string city, DateTime date);
    }
}