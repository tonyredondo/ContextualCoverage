using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScopeAgent.Coverage;

namespace ContextualCoverage.Api
{
    public class CoverageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private int _coverageSession;

        public CoverageMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            CoverageReporter.Handler = new ScopeCoverageEventHandler(new CoverageSettings());
            _next = next;
            _logger = loggerFactory.CreateLogger<CoverageMiddleware>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Query.ContainsKey("cover"))
                return InvokeWithCoverageAsync(context);
            return _next(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task InvokeWithCoverageAsync(HttpContext context)
        {
            CoverageReporter.Handler.StartAsyncSession(Guid.NewGuid());
            await _next(context).ConfigureAwait(false);
            var session = CoverageReporter.Handler.EndAsyncSession();
            if (session != null && Interlocked.CompareExchange(ref _coverageSession, 1, 0) == 0)
            {
                _logger.LogInformation("Saving coverage info");
                var json = JsonConvert.SerializeObject(session, Formatting.Indented);
                File.WriteAllText($"coverage.json", json);
            }
        }
    }
}
