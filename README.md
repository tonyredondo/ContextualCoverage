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

For performance analysis we ran:

```bash
wrk -c 8 -d 10 -t 4 https://localhost:5001/weatherforecast
```
and
```bash
wrk -c 8 -d 10 -t 4 https://localhost:5001/weatherforecast?cover
```

We did that 10 times with 3 possible scenarios:

1) **No Coverage**: This represent the original build without the `CoverageMiddleware`.
2) **With Coverage Disabled**: This scenario contains the patched build with the `CoverageMiddleware` active but without opening a coverage session (not `query=cover`)
3) **With Coverage Enabled**: This scenario has the patched build with the `CoverageMiddleware`active and an active session for the request with `query=cover`.

### The results in a 13 inch MacBook Pro of late 2014 are:

|                            | Avg Req/Sec | Std. Dev. |   %    |  Delta  |
|:--------------------------:|:-----------:|:---------:|:------:|:-------:|
| **No Coverage**            | 21665.11    | 385.31    | 100%   | -       |
| **With Coverage Disabled** | 20982.86    | 680.18    | 96.85% | -3.15%  |
| **With Coverage Enabled**  | 16990.16    | 242.84    | 78.42% | -21.58% |


### The results in a 15 inch MacBook Pro of 2018 are:

|                            | Avg Req/Sec | Std. Dev. |   %    |  Delta  |
|:--------------------------:|:-----------:|:---------:|:------:|:-------:|
| **No Coverage**            | 48072.39    | 472.12    | 100%   | -       |
| **With Coverage Disabled** | 47939.73    | 422.92    | 99.72% | -0.28%  |
| **With Coverage Enabled**  | 41821.98    | 711.15    | 87.24% | -12.76% |

Using the same machine but, running:

```bash
wrk -c 12 -d 10 -t 6 https://localhost:5001/weatherforecast
```

|                            | Avg Req/Sec | Std. Dev. |   %    |  Delta  |
|:--------------------------:|:-----------:|:---------:|:------:|:-------:|
| **No Coverage**            | 48459.94    | 388.34    | 100%   | -       |
| **With Coverage Disabled** | 48456.47    | 376.71    | 99.99% | -0.01%  |
| **With Coverage Enabled**  | 46529.14    | 447.39    | 96.02% | -3.98%  |


### The results in a Ryzen Threadripper 2950X 16-Core Processor (Windows 10 + WSL), running:

```bash
wrk -c 32 -d 10 -t 16 https://localhost:5001/weatherforecast
```

|                            | Avg Req/Sec | Std. Dev. |   %    |  Delta  |
|:--------------------------:|:-----------:|:---------:|:------:|:-------:|
| **No Coverage**            | 73888.35    | 1438.84   | 100%   | -       |
| **With Coverage Disabled** | 73236.34    | 2258.05   | 99.12% | -0.88%  |
| **With Coverage Enabled**  | 64465.51    | 1830.63   | 88.02% | -11.98% |
