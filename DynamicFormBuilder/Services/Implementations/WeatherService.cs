using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace DynamicFormBuilder.Services.Implementations
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _baseUrl = configuration["WeatherApi:BaseUrl"];
        }

        public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city)
        {
            try
            {
                var url = $"{_baseUrl}/current.json?key={_apiKey}&q={city}&aqi=no";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Weather API Error: {response.StatusCode} - {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<CurrentWeatherResponse>(content, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching current weather: {ex.Message}", ex);
            }
        }

        public async Task<ForecastWeatherResponse> GetForecastWeatherAsync(string city, int days = 7)
        {
            try
            {
                // WeatherAPI allows up to 14 days forecast on paid plans, 3 days on free
                if (days > 14) days = 14;
                if (days < 1) days = 1;

                var url = $"{_baseUrl}/forecast.json?key={_apiKey}&q={city}&days={days}&aqi=no";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Weather API Error: {response.StatusCode} - {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<ForecastWeatherResponse>(content, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching forecast weather: {ex.Message}", ex);
            }
        }

        public async Task<HistoricalWeatherResponse> GetHistoricalWeatherAsync(string city, DateTime date)
        {
            try
            {
                var dateStr = date.ToString("yyyy-MM-dd");
                var url = $"{_baseUrl}/history.json?key={_apiKey}&q={city}&dt={dateStr}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Weather API Error: {response.StatusCode} - {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<HistoricalWeatherResponse>(content, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching historical weather: {ex.Message}", ex);
            }
        }


    }
    }
