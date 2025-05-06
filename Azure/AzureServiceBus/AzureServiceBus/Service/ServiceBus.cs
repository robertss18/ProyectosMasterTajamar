using Azure.Messaging.ServiceBus;
using System.Text;

namespace AzureServiceBus.Service
{
    public interface IServiceBusService
    {
        Task SendMessageToQueueAsync(string message);
        Task SendMessageToTopicAsync(string message);
        Task<string> ReceiveMessageFromQueueAsync();
        Task<string> ReceiveMessageFromSubscriptionAsync();
    }

    public class ServiceBusService : IServiceBusService
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly string _topicName;
        private readonly string _subscriptionName;

        public ServiceBusService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureServiceBus:ConnectionString"]!;
            _queueName = configuration["AzureServiceBus:QueueName"]!;
            _topicName = configuration["AzureServiceBus:TopicName"]!;
            _subscriptionName = configuration["AzureServiceBus:SubscriptionName"]!;
        }

        public async Task SendMessageToQueueAsync(string message)
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(_queueName);
            ServiceBusMessage busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));
            await sender.SendMessageAsync(busMessage);
        }

        public async Task SendMessageToTopicAsync(string message)
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(_topicName);
            ServiceBusMessage busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message));
            await sender.SendMessageAsync(busMessage);
        }

        public async Task<string> ReceiveMessageFromQueueAsync()
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusReceiver receiver = client.CreateReceiver(_queueName);
            ServiceBusReceivedMessage busMessage = await receiver.ReceiveMessageAsync();
            return busMessage != null ? Encoding.UTF8.GetString(busMessage.Body) : "No messages";
        }

        public async Task<string> ReceiveMessageFromSubscriptionAsync()
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusReceiver receiver = client.CreateReceiver(_topicName, _subscriptionName);
            ServiceBusReceivedMessage busMessage = await receiver.ReceiveMessageAsync();
            return busMessage != null ? Encoding.UTF8.GetString(busMessage.Body) : "No messages";
        }
    }
}