using System.Threading.Tasks;
using Xunit;

namespace ContextualCoverage.Tests
{
    public class WeatherTests
    {
        [Fact]
        public async Task WeatherTest()
        {
            var temp = await WeatherService.GetCurrentTempAsync().ConfigureAwait(false);
                Assert.InRange(temp, 10, 100);
        }

        [Fact]
        public async Task CachedWeatherTest()
        {
            var temp = await CachedWeatherService.GetCurrentTempAsync().ConfigureAwait(false);
            Assert.InRange(temp, 10, 100);

            var temp2 = await CachedWeatherService.GetCurrentTempAsync().ConfigureAwait(false);
            Assert.Equal(temp, temp2);
        }
    }
}
