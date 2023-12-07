using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IWent.Services.Notifications;

public class NotificationClient : INotificationClient
{
    private readonly ServiceBusClient _busClient;
    private readonly ILogger<NotificationClient> _logger;

    public NotificationClient(ServiceBusClient busClient, ILogger<NotificationClient> logger)
    {
        _busClient = busClient;
        _logger = logger;
    }

    public async Task SendMessageAsync<T>(T message, string queueName, CancellationToken cancellationToken)
    {
        var sender = _busClient.CreateSender(queueName);

        var serializedMessage = JsonConvert.SerializeObject(message);
        var queueMessage = new ServiceBusMessage(new BinaryData(serializedMessage));

        try
        {
            await sender.SendMessageAsync(queueMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown when sending a message to the queue '{QueueName}'", queueName);
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }
}
