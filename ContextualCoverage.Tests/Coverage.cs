using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using ScopeAgent.Coverage;

namespace ContextualCoverage.Tests
{
    public static class Coverage
    {
        public static IDisposable CreateSession([CallerMemberName]string sessionName = null)
        {
            return DisposableCoverage.CreateSession((name, session) =>
            {
                if (session is ScopeAgent.Coverage.Results.CoverageSession scopeSession && scopeSession.Files.Count > 0)
                {
                    var json = JsonConvert.SerializeObject(session, Formatting.Indented);
                    File.WriteAllText($"coverage_{name}.json", json);
                }
            }, sessionName);
        }
    }
}
