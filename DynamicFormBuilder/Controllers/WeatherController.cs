using DynamicFormBuilder.Models;
using DynamicFormBuilder.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormBuilder.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                ModelState.AddModelError("", "Please enter a city name");
                return View("Index");
            }

            try
            {
                var viewModel = new WeatherViewModel
                {
                    City = city,
                    CurrentWeather = await _weatherService.GetCurrentWeatherAsync(city),
                    ForecastWeather = await _weatherService.GetForecastWeatherAsync(city, 7),
                    HistoricalWeather = await _weatherService.GetHistoricalWeatherAsync(city, DateTime.Now.AddDays(-7))
                };

                return View("WeatherDisplay", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather(string city)
        {
            try
            {
                var weather = await _weatherService.GetCurrentWeatherAsync(city);
                return Json(weather);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetForecast(string city, int days = 7)
        {
            try
            {
                var forecast = await _weatherService.GetForecastWeatherAsync(city, days);
                return Json(forecast);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHistorical(string city, string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return BadRequest(new { error = "Invalid date format" });
                }

                var historical = await _weatherService.GetHistoricalWeatherAsync(city, parsedDate);
                return Json(historical);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
    
}
