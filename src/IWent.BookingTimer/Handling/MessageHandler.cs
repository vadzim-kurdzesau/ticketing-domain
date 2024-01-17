using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using IWent.BookingTimer.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IWent.BookingTimer.Handling;

internal class MessageHandler : BackgroundService
{
    private readonly IBackgroundQueue<BookingTimerMessage> _processingQueue;
    private readonly ConcurrentDictionary<string, BookingTimer> _timers;
    private readonly ILogger<MessageHandler> _logger;

    public MessageHandler(IBackgroundQueue<BookingTimerMessage> processingQueue, ILogger<MessageHandler> logger)
    {
        _processingQueue = processingQueue;
        _timers = new ConcurrentDictionary<string, BookingTimer>();
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message handler has been started.");
        while (stoppingToken.IsCancellationRequested)
        {
            var message = await _processingQueue.DequeueAsync(stoppingToken);

            if (_timers.TryGetValue(message.BookingNumber, out var existingTimer))
            {
                if (message.Action != TimerAction.Stop)
                {
                    _logger.LogWarning("Received a 'Start' request for already running timer '{TimerId}'.", message.BookingNumber);
                    continue;
                }

                await existingTimer.StopAsync(stoppingToken);
                _logger.LogInformation("Stopped the timer for booking '{BookingId}'.", message.BookingNumber);
                continue;
            }

            if (message.Action == TimerAction.Stop)
            {
                _logger.LogWarning("Received a stopping request for non-existing timer '{TimerId}'.", message.BookingNumber);
                continue;
            }

            var newBookingTimer = new BookingTimer(TimeSpan.FromMinutes(15));
            await newBookingTimer.StartAsync(stoppingToken);

            _timers.TryAdd(message.BookingNumber, newBookingTimer);
            _logger.LogInformation("Started the timer for booking '{BookingId}'.", message.BookingNumber);
        }

        _logger.LogInformation("Message handler has been stopped.");
    }
}
