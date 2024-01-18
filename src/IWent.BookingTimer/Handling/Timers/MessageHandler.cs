using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using IWent.BookingTimer.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IWent.BookingTimer.Handling.Timers;

internal class MessageHandler : BackgroundService
{
    private readonly IBackgroundQueue<BookingTimerMessage> _processingQueue;
    private readonly ITimerFactory _timerFactory;
    private readonly ConcurrentDictionary<string, BookingTimer> _timers;
    private readonly ILogger<MessageHandler> _logger;

    public MessageHandler(IBackgroundQueue<BookingTimerMessage> processingQueue, ITimerFactory timerFactory, ILogger<MessageHandler> logger)
    {
        _processingQueue = processingQueue;
        _timerFactory = timerFactory;
        _timers = new ConcurrentDictionary<string, BookingTimer>();
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message handler has been started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ListenForMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // Thrown exceptions should not stop the message handler from working.
                _logger.LogError(ex, "An exception was thrown during message handling.");
            }
        }

        _logger.LogInformation("Message handler has been stopped.");
    }

    private async Task ListenForMessagesAsync(CancellationToken stoppingToken)
    {
        var message = await _processingQueue.DequeueAsync(stoppingToken);

        if (_timers.TryGetValue(message.BookingNumber, out var existingTimer))
        {
            if (message.Action != TimerAction.Stop)
            {
                _logger.LogWarning("Received a 'Start' request for already running timer '{TimerId}'.", message.BookingNumber);
                return;
            }

            _timers.TryRemove(message.BookingNumber, out _);
            await existingTimer.StopAsync(stoppingToken);
            _logger.LogInformation("Stopped the timer for booking '{BookingId}'.", message.BookingNumber);
            return;
        }

        if (message.Action == TimerAction.Stop)
        {
            _logger.LogWarning("Received a stopping request for non-existing timer '{TimerId}'.", message.BookingNumber);
            return;
        }

        var newBookingTimer = _timerFactory.Create(message.BookingNumber);
        newBookingTimer.Expired += RemoveTimer;
        await newBookingTimer.StartAsync(stoppingToken);

        _timers.TryAdd(message.BookingNumber, newBookingTimer);
        _logger.LogInformation("Started the timer for booking '{BookingId}'.", message.BookingNumber);
    }

    private void RemoveTimer(object? sender, TimerExpiredEventArgs eventArgs)
    {
        if (_timers.TryRemove(eventArgs.BookingId, out var timer))
        {
            timer.Expired -= RemoveTimer;
            _logger.LogDebug("Removed timer '{TimerId}' from the running ones.", eventArgs.BookingId);
        }
    }
}
