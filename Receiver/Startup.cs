using SharedLibrary.Services;

namespace Receiver
{
    public class Startup
    {
        // Services
        private readonly ServiceBusService _busService;

        public Startup(ServiceBusService busService)
        {
            _busService = busService;
        }

        public async Task Run()
        {
            try
            {
                // starts processing messages
                await _busService.StartProcessingAsync();

                Console.WriteLine("Listening for messages... Press [Enter] to exit.");

                while (true)
                {
                    // poll every 5 seconds
                    await Task.Delay(5000);

                    // gets the errors
                    var errors = _busService.GetErrorMessages();
                    if (errors.Any())
                    {
                        Console.WriteLine($"Errors: {string.Join(", ", errors)}");
                        _busService.ClearErrors();
                        break;
                    }

                    // gets the messages
                    var messages = _busService.GetReceivedMessages().ToList();
                    foreach (var message in messages)
                    {
                        Console.WriteLine($"New order received: {message.Id}, {message.ProductName}, {message.Quantity}, {message.Price}");
                        _busService.ClearMessages();
                    }

                    // exit from the app
                    if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
                        break;
                }

                // stops processing messages
                await _busService.StopProcessingAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
