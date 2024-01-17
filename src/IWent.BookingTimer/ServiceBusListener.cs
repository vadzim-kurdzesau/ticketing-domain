using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IWent.BookingTimer.Configuration;
using IWent.BookingTimer.Handling;
using IWent.BookingTimer.Messages;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IWent.BookingTimer;

public class ServiceBusListener : BackgroundService
{
    private readonly ServiceBusReceiver _receiver;
    private readonly IBackgroundQueue<BookingTimerMessage> _processingQueue;
    private readonly ILogger<ServiceBusListener> _logger;

    public ServiceBusListener(
        IAzureClientFactory<ServiceBusReceiver> clientFactory,
        IBackgroundQueue<BookingTimerMessage> backgroundQueue,
        BusConfiguration configuration,
        ILogger<ServiceBusListener> logger)
    {
        _receiver = clientFactory.CreateClient(configuration.QueueName);
        _processingQueue = backgroundQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Service bus listener has been started .");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await foreach (var message in _receiver.ReceiveMessagesAsync(stoppingToken))
                {
                    _logger.LogInformation("Received the message '{MessageId}'.", message.MessageId);

                    var bookingMessage = message.Body.ToObjectFromJson<BookingTimerMessage>();

                    await _processingQueue.EnqueueAsync(bookingMessage, stoppingToken);

                    _logger.LogInformation("Successfully added the message '{MessageId}' to the processing queue.", message.MessageId);
                }
            }
            catch (Exception ex)
            {
                // Thrown exceptions should not stop the application from working.
                _logger.LogError(ex, "An exception was thrown during the service bus messages receiving.");
            }
        }

        _logger.LogInformation("Service bus listener has been stopped.");
    }
}
