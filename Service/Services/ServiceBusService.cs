using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLibrary.Configuration;
using SharedLibrary.Models;
using System.Text;

namespace SharedLibrary.Services
{
    /// <summary>
    /// Provides data manipulation functions for the service-bus.
    /// </summary>
    public class ServiceBusService
    {
        // private variables
        private readonly ServiceBusSender      _sender;
        private readonly ServiceBusProcessor   _processor;
        private readonly List<Order>           _receivedMessages;
        private readonly List<string>          _errorMessages;   

        public ServiceBusService(ServiceBusClient client, IOptions<ServiceBusOptions> options)
        {
            // settings
            var settings = options.Value;

            // creates the sender
            _sender      = client.CreateSender(settings.QueueName);

            // creates processor and config options
            _processor = client.CreateProcessor(settings.QueueName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,    // will manually complete messages
                MaxConcurrentCalls   = 1         // process up to 1 messages in parallel
            });

            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync   += ErrorHandler;

            // message storage
            _receivedMessages = new List<Order>();

            // error storage
            _errorMessages    = new List<string>();
        }

        /// <summary>
        /// Handles the receiving messages.
        /// </summary>
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            // gets the message body
            string jsonString = Encoding.UTF8.GetString(args.Message.Body);

            // validates before return the deserialization
            if (!string.IsNullOrEmpty(jsonString))
            {
                // de-serializes the message
                var order = JsonConvert.DeserializeObject<Order>(jsonString);

                // adds messages to the list
                _receivedMessages.Add(order);
            }

            // completes the process
            await args.CompleteMessageAsync(args.Message);
        }

        /// <summary>
        /// Handles the errors.
        /// </summary>
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // adds messages to the list
            _errorMessages.Add(args.Exception.Message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends message to the queue.
        /// </summary>
        /// <param name="busMessage">The message to be sent to the queue.</param>
        public async Task SendMessageAsync<T>(T busMessage)
        {
            // serializes the message to json
            string jsonMessage = JsonConvert.SerializeObject(busMessage);

            // converts to byte array
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));

            // sends the message to the queue
            await _sender.SendMessageAsync(message);
        }

        /// <summary>
        /// Starts the message processing.
        /// </summary>
        public Task StartProcessingAsync()              => _processor.StartProcessingAsync();

        /// <summary>
        /// Stops the message processing.
        /// </summary>
        public Task StopProcessingAsync()               => _processor.StopProcessingAsync();

        /// <summary>
        /// The received messages list.
        /// </summary>
        public IEnumerable<Order> GetReceivedMessages() => _receivedMessages;

        /// <summary>
        /// The received error messages list.
        /// </summary>
        public IEnumerable<string> GetErrorMessages()   => _errorMessages;

        /// <summary>
        /// Clears the received messages list.
        /// </summary>
        public void ClearMessages()                     => _receivedMessages.Clear();

        /// <summary>
        /// Clears the error messages list.
        /// </summary>
        public void ClearErrors()                       => _errorMessages.Clear();
    }
}
