using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IWent.BookingTimer.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

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
        using (var timer = new PeriodicTimer(_expiresAfter))
        {
            await timer.WaitForNextTickAsync(stoppingToken);
            _logger.LogInformation("Booking timer '{BookingId}' has expired after {Seconds} seconds.", _bookingId, _expiresAfter.Seconds);
        }

        var bookingExpiredMessage = new BookingExpiredMessage
        {
            BookingId = _bookingId
        };

        var messageBody = JsonSerializer.Serialize(bookingExpiredMessage);
        var message = new ServiceBusMessage(messageBody);

        const int RetryAttempts = 3;
        try
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retryCount: RetryAttempts, retryAttempt =>
                {
                    _logger.LogWarning("An error occured during the booking timer expiration message sending. Attempting to resend: #{Attempt}.", retryAttempt);
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                });

            await retryPolicy.ExecuteAsync(() => _busSender.SendMessageAsync(message, CancellationToken.None));

            _logger.LogDebug("Successfully sent the booking timer expiration message with the booking ID: '{BookingId}'.", _bookingId);
        }
        catch (Exception ex)
        {
            // Error in the message sending should not break the application
            _logger.LogError(ex, "An exception was thrown during the booking timer expiration message sending after {NumberOfAttempts} attempts.", RetryAttempts);
        }

        OnExpired();
    }

    protected virtual void OnExpired()
    {
        Expired?.Invoke(this, new TimerExpiredEventArgs { BookingId = _bookingId });
    }
}
