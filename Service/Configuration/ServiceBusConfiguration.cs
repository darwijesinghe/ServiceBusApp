using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedLibrary.Services;

namespace SharedLibrary.Configuration
{
    /// <summary>
    /// Provides service-bus configuration options.
    /// </summary>
    public static class ServiceBusConfiguration
    {
        public static void AddServiceBusClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind configuration
            services.Configure<ServiceBusOptions>(configuration.GetSection("AzureServiceBus"));

            // Register service-bus client
            services.AddSingleton(sp =>
            {
                // settings
                var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

                // options
                var clientOptions = new ServiceBusClientOptions
                {
                    RetryOptions = new ServiceBusRetryOptions
                    {
                        Mode       = ServiceBusRetryMode.Exponential,   // retry mode
                        MaxRetries = 3,                                 // maximum retry attempts
                        Delay      = TimeSpan.FromSeconds(1),           // initial retry delay
                        MaxDelay   = TimeSpan.FromSeconds(30)           // maximum retry delay
                    },
                    TransportType  = ServiceBusTransportType.AmqpTcp,   // transport type
                    // Identifier  = "my-bus-client"                    // custom identifier for the client when logging and debugging
                };

                return new ServiceBusClient(options.ConnectionString, clientOptions);
            });

            // Register data managing services
            services.AddSingleton<ServiceBusService>();
        }
    }
}
