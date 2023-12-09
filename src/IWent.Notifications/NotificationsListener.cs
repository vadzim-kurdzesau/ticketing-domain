using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IWent.Messages;
using IWent.Notifications.Handling;
using IWent.Notifications.Messaging;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IWent.Notifications;

internal class NotificationsListener : BackgroundService
{
    private readonly INotificationHandlersFactory _handlersFactory;
    private readonly ILogger<NotificationsListener> _logger;
    private readonly ServiceBusReceiver _receiver;

    public NotificationsListener(IAzureClientFactory<ServiceBusReceiver> clientFactory, IMessageQueueConfiguration queueConfiguration, INotificationHandlersFactory handlersFactory, ILogger<NotificationsListener> logger)
    {
        _handlersFactory = handlersFactory;
        _logger = logger;
        _receiver = clientFactory.CreateClient(queueConfiguration.QueueName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting receiving the incoming from queue messages.", nameof(NotificationsListener));

        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: handle this message
            var message = await _receiver.ReceiveMessageAsync(maxWaitTime: TimeSpan.FromSeconds(5), cancellationToken: stoppingToken);
            try
            {
                if (message == null)
                {
                    continue;
                }

                await HandleMessageAsync(message, stoppingToken);

                await _receiver.CompleteMessageAsync(message, CancellationToken.None);
            }
            catch (Exception ex)
            {
                await _receiver.DeadLetterMessageAsync(message, cancellationToken: stoppingToken);
                _logger.LogError(ex, "An exception was thrown during the {Listener} execution.", nameof(NotificationsListener));
            }
        }

        _logger.LogInformation("Stopping the {Listener}.", nameof(NotificationsListener));
    }

    private Task HandleMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received a message with the ID '{ID}'.", message.MessageId);

        var json = message.Body.ToString();
        var notification = JsonConvert.DeserializeObject<Notification>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        if (notification == null)
        {
            _logger.LogWarning("Received message with the ID '{ID}' is not of a '{Notification}' type.", nameof(Notification));
            return Task.CompletedTask;
        }

        var handler = _handlersFactory.Create(notification);
        return handler.HandleAsync(notification, cancellationToken);
    }
}
