using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContextualCoverage.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : Controller
    {
        public async Task<double> Index()
        {
            var currentTemp = await CachedWeatherService.GetCurrentTempAsync().ConfigureAwait(false);
            return Util.ToFahrenheit(currentTemp);
        }
    }
}
