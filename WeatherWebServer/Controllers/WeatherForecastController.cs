using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenMeteo;
using WeatherWebServer.Services;

namespace WeatherWebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherRepository _weatherRepository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IHttpClientFactory httpClientFactory,
            WeatherRepository weatherRepository,
            ILogger<WeatherForecastController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _weatherRepository = weatherRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<WeatherForecast> Get(double lat = -33.52, double lon = 151.12)
        {
            _logger.LogInformation("Getting forecast");
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,windspeed_10m&hourly=temperature_2m,relativehumidity_2m,windspeed_10m";

            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<WeatherForecast>(url);
            return response;
        }
        
        [HttpGet]
        [Route("forecast")]
        public WeatherForecast Get2(string location) => 
            _weatherRepository.Get(location);
    }
}
