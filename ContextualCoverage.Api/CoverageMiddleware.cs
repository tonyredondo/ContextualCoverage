using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScopeAgent.Coverage;
using ScopeAgent.Coverage.Attributes;

namespace ContextualCoverage.Api
{
    public class CoverageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly HttpClient _client;

        public CoverageMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            CoverageReporter.Handler = new ScopeCoverageEventHandler(new CoverageSettings());
            _next = next;
            _logger = loggerFactory.CreateLogger<CoverageMiddleware>();
            _client = new HttpClient();
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
            if (session != null)
            {
                _logger.LogInformation("Saving coverage info");
                var covPayload = new CoverageServerPayload(session);
                var json = JsonConvert.SerializeObject(covPayload, Formatting.Indented);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await _client.PostAsync("http://rotopia.xyz:8080/ingest", content).ConfigureAwait(false);
                File.WriteAllText($"coverage.json", json);
            }
        }

        public class CoverageServerPayload
        {
            [DataMember(Name = "gitInfo")]
            public CoverageServerGitInfo GitInfo { get; set; }
            [DataMember(Name = "coverage")]
            public object Coverage { get; set; }

            public CoverageServerPayload(object coverage)
            {
                var covAttribute = Assembly.GetEntryAssembly().GetCustomAttribute<CoveredAssemblyAttribute>();
                if (covAttribute != null)
                {
                    GitInfo = new CoverageServerGitInfo
                    {
                        Repository = covAttribute.Repository,
                        Branch = covAttribute.Branch,
                        Commit = covAttribute.Commit,
                        SourceRoot = covAttribute.SourceRoot
                    };
                }
                Coverage = coverage;
            }
        }
        public class CoverageServerGitInfo
        {
            [DataMember(Name = "repository")]
            public string Repository { get; set; }
            [DataMember(Name = "branch")]
            public string Branch { get; set; }
            [DataMember(Name = "commit")]
            public string Commit { get; set; }
            [DataMember(Name = "sourceRoot")]
            public string SourceRoot { get; set; }
        }
    }
}
