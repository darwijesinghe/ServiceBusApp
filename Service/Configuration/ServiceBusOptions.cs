namespace SharedLibrary.Configuration
{
    /// <summary>
    /// Provides configuration options.
    /// </summary>
    public class ServiceBusOptions
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the message queue
        /// </summary>
        public string QueueName        { get; set; }
    }
}
