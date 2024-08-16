// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
IServiceCollection services = builder.Services;

services
    .AddHttpClient("weather", client => new Uri("https:/localhost:71000"))
    //.AddResilienceHandler();

var httpClient = builder.Build().Services.GetRequiredService<IHttpClientFactory>().CreateClient();

