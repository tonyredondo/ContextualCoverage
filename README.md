# Contextual Coverage Example (.NET)

The requirements to run this example is to have installed [`.NET Core 3.0`](https://dotnet.microsoft.com/download) sdk.

## Run Unit Tests with Contextual Coverage

To run the tests using the contextual coverage collector of Scope, execute the following:

```bash
dotnet test --collect ScopeAgent
```

This will run the test suite and create one coverage file for each test in the internal Scope format:

| Coverage files |
|:--------------|
| ContextualCoverage.Tests/bin/Debug/netcoreapp3.0/coverage_CachedWeatherTest.json |
| ContextualCoverage.Tests/bin/Debug/netcoreapp3.0/coverage_FactorialTest.json |
| ContextualCoverage.Tests/bin/Debug/netcoreapp3.0/coverage_FibonacciTest.json |
| ContextualCoverage.Tests/bin/Debug/netcoreapp3.0/coverage_WeatherTest.json |

## Run an ASP.NET Web Api with Request based contextual coverage

In order to run the ASP.NET Web Api example first we must compile the `ContextualCoverage.Api` project then patch the binaries using the `ContextualCoverage.Processor` project before running the Api. 

```bash
dotnet build -c Release
cd ContextualCoverage.Processor
./run.sh
```

Then we run the `ContextualCoverage.Api` withoud rebuilding:

```bash 
cd ../ContextualCoverage.Api
dotnet run -c Release --no-build
```

This will start the Web Api, you can try by pointing the browser to: `https://localhost:5001/weatherforecast`. This will give you the current weather forecast for `London` in Fahrenheit.

Now we can enable a coverage of a request by navigating to: `https://localhost:5001/weatherforecast?cover`

This will create a coverage file for that request in:

| Coverage files |
|:--------------|
| ContextualCoverage.Api/coverage.json |

## Performance 

We ran the command `wrk -c 8 -d 10 -t 4 https://localhost:5001/weatherforecast` 10 times to analyze 3 possible scenarios, the results are:

|                        | Avg Req/Sec | Std. Dev. |   %    |  Delta  |
|:----------------------:|:-----------:|:---------:|:------:|:-------:|
| No Coverage            | 21665.11    | 385.31    | 100%   | -       |
| With Coverage Disabled | 20982.86    | 680.18    | 96,85% | -3,15%  |
| With Coverage Enabled  | 16990.16    | 242.84    | 78,42% | -21,58% |

Both Enabling and Disabling the coverage of the request is done at runtime.