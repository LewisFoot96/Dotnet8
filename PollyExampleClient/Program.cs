// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Polly;

Console.WriteLine("Hello, World!");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
IServiceCollection services = builder.Services;

//Can add standard resliancy handler that has these 5 strategies already.
//Can use the configure extension to change the options
//Can add hedging handler, long retries can send mutliple concurrrent requests
services
    .AddHttpClient("weather", client => new Uri("https:/localhost:5150"))
    .AddResilienceHandler("demo", builder =>
    {
        //100 concurrent requests at any given time, prevent ddos.
        builder.AddConcurrencyLimiter(100);

        //Request no longer than a second
        builder.AddTimeout(TimeSpan.FromSeconds(1));

        //Add retry resiliance 
        builder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 5,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            Delay = TimeSpan.FromSeconds(1)
        });

        //Detect outage is remote service and then stop requests. 
        builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
        {
            SamplingDuration = TimeSpan.FromSeconds(5),
            FailureRatio = 0.9,
            MinimumThroughput = 5,
            BreakDuration = TimeSpan.FromSeconds(5)
        });
    });

var httpClient = builder
    .Build().Services
    .GetRequiredService<IHttpClientFactory>()
    .CreateClient(); //Resolving client and resiliance handler

for (int i = 0; i <= 100; i++)
{
    var result = await httpClient.GetAsync("https://localhost:5150");
    Console.WriteLine(result);
}

