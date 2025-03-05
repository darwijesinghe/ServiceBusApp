using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Receiver;
using SharedLibrary.Configuration;
using SharedLibrary.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a Host with Dependency Injection
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureLogging(log =>
            {
                log.ClearProviders();   // Clears default logging providers
                log.AddConsole();       // Keep console logging
            })
           .ConfigureServices((context, services) =>
           {
               // Bind configuration
               services.Configure<ServiceBusOptions>(context.Configuration.GetSection("AzureServiceBus"));

               // Register Service Bus Client from the shared library
               services.AddServiceBusClient(context.Configuration);

               // Register data managing services
               services.AddSingleton<ServiceBusService>();
           })
           .Build();

        // Automatically resolve and inject any dependencies required by the Startup class's constructor.
        // This helps in creating instances of classes that have dependencies registered in the DI container.

        var service = ActivatorUtilities.CreateInstance<Startup>(host.Services);
        await service.Run();
    }
}