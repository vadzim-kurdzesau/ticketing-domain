using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IWent.Services.Constants;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IWent.Services.Notifications;

public class NotificationClient : INotificationClient
{
    private readonly ServiceBusClient _busClient;
    private readonly ILogger<NotificationClient> _logger;

    public NotificationClient(IAzureClientFactory<ServiceBusClient> clientFactory, ILogger<NotificationClient> logger)
    {
        _busClient = clientFactory.CreateClient(ServiceBusClientNames.DefaultBusClient);
        _logger = logger;
    }

    public async Task SendMessageAsync<T>(T message, string queueName, CancellationToken cancellationToken)
    {
        var serializedMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        var queueMessage = new ServiceBusMessage(new BinaryData(serializedMessage));

        try
        {
            await using var sender = _busClient.CreateSender(queueName);
            await sender.SendMessageAsync(queueMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown when sending a message to the queue '{QueueName}'", queueName);
        }
    }
}
