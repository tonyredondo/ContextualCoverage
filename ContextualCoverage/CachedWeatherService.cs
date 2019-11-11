using System;
using System.Threading.Tasks;

namespace ContextualCoverage
{
    public static class CachedWeatherService
    {
        private static object _locker = new object();
        private static Task<double> _cachedTemp;

        public static Task<double> GetCurrentTempAsync()
        {
            if (_cachedTemp != null) return _cachedTemp;
            lock (_locker)
            {
                if (_cachedTemp != null) return _cachedTemp;
                return _cachedTemp = WeatherService.GetCurrentTempAsync();
            }
        }
    }
}
