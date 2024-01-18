using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IWent.BookingTimer.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IWent.BookingTimer.Handling.Timers;

public class BookingTimer : BackgroundService
{
    private readonly ServiceBusSender _busSender;
    private readonly string _bookingId;
    private readonly TimeSpan _expiresAfter;
    private readonly ILogger<BookingTimer> _logger;

    public BookingTimer(ServiceBusSender busSender, string bookingId, TimeSpan expiresAfter, ILogger<BookingTimer> logger)
    {
        _busSender = busSender;
        _bookingId = bookingId;
        _expiresAfter = expiresAfter;
        _logger = logger;
    }

    /// <summary>
    /// Event that is triggered when the timer expires.
    /// </summary>
    public event EventHandler<TimerExpiredEventArgs>? Expired;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_expiresAfter);

        try
        {
            await timer.WaitForNextTickAsync(stoppingToken);
        }
        catch (TaskCanceledException) // If the timer was manually cancelled
        {
            _logger.LogInformation("AAAAAAAAAAAAAAAAAAA");
            return;
        }

        var bookingExpiredMessage = new BookingExpiredMessage
        {
            BookingId = _bookingId
        };

        var messageBody = JsonSerializer.Serialize(bookingExpiredMessage);
        var message = new ServiceBusMessage(messageBody);

        try
        {
            await _busSender.SendMessageAsync(message, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown during the message send.");
        }

        OnExpired();
    }

    protected virtual void OnExpired()
    {
        Expired?.Invoke(this, new TimerExpiredEventArgs { BookingId = _bookingId });
    }
}
