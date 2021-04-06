using App.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMetrics.Metrics;

namespace TestMetrics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MetricsHelper _metricsHelper;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            MetricsHelper metricsHelper)
        {
            _logger = logger;
            _metricsHelper = metricsHelper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _metricsHelper.IncrementCounter("forecast");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
