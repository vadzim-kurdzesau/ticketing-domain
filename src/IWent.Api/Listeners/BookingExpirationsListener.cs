﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IWent.BookingTimer.Messages;
using IWent.Services;
using IWent.Services.Constants;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IWent.Api.Listeners;

public class BookingExpirationsListener : BackgroundService
{
    private readonly ServiceBusReceiver _receiver;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<BookingExpirationsListener> _logger;

    public BookingExpirationsListener(IAzureClientFactory<ServiceBusReceiver> clientFactory, IPaymentService paymentService, ILogger<BookingExpirationsListener> logger)
    {
        _receiver = clientFactory.CreateClient(ServiceBusClientNames.ExpiredTimersReceiver);
        _paymentService = paymentService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Booking expirations listener has been started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await foreach (var message in _receiver.ReceiveMessagesAsync(stoppingToken))
                {
                    var expirationMessage = message.Body.ToObjectFromJson<BookingExpiredMessage>();

                    _logger.LogInformation("Received the expired booking message with the ID '{BookingId}'.", message.MessageId);

                    await _paymentService.FailOrderPaymentAsync(expirationMessage.BookingId, stoppingToken);

                    _logger.LogInformation("Successfully failed booking '{BookingId}' and released all booked seats.", expirationMessage.BookingId);
                }
            }
            catch (Exception ex)
            {
                // Thrown exceptions should not stop the listener from working.
                _logger.LogError(ex, "An exception was thrown during the booking expiration listener messages receiving.");
            }
        }
    }
}