# AzureServiceBus

## Project Purpose
This is a practice project designed to understand how Azure Service Bus works. The goal of this project is to gain hands-on experience with Azure Service Bus, including:

- Sending messages from an API.
- Processing messages asynchronously using a console application.
- Managing message queues efficiently.

## Project Structure
1. **API Project** 
  - Exposes an endpoint to send messages to Azure Service Bus.
2. **Console App** 
  - Listens for messages and processes them upon receiving.
3. **Shared Library** 
  - Contains common functionality for interacting with Azure Service Bus (e.g., message sending, processing, error handling).

## Packages
- Azure.Messaging.ServiceBus
- Newtonsoft.Json

## Configuration
1. Update the **Azure Service Bus connection string** and **queue name** in the configuration files of both the API and Console App.
2. Run the **API project** to send messages to the specified queue.
3. Start the **Console App** to receive and process messages from the queue.

## Contributors
Darshana Wijesinghe

## Support
Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License
This project is licensed under the terms of the **MIT** license.