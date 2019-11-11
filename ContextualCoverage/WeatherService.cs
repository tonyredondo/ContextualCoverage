using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContextualCoverage
{
    public static class WeatherService
    {
        private static HttpClient _httpClient = new HttpClient();

        public static async Task<double> GetCurrentTempAsync()
        {
            var response = await _httpClient.GetAsync("https://www.metaweather.com/api/location/2487956/").ConfigureAwait(false);
            var jsonObj = (JToken)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return (double)jsonObj["consolidated_weather"].First["the_temp"].ToObject(typeof(double));
        }
    }
}
