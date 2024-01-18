using System;
using Azure.Messaging.ServiceBus;
using IWent.BookingTimer.Configuration;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IWent.BookingTimer.Handling.Timers;

public class TimerFactory : ITimerFactory
{
    private readonly ITimerConfiguration _timerConfiguration;
    private readonly IAzureClientFactory<ServiceBusSender> _clientFactory;
    private readonly IServiceProvider _serviceProvider;

    public TimerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _timerConfiguration = _serviceProvider.GetRequiredService<ITimerConfiguration>();
        _clientFactory = _serviceProvider.GetRequiredService<IAzureClientFactory<ServiceBusSender>>();
    }

    public BookingTimer Create(string bookingId)
    {
        var busSender = _clientFactory.CreateClient(Constants.ServiceBusSenderName);
        var logger = _serviceProvider.GetRequiredService<ILogger<BookingTimer>>();
        return new BookingTimer(busSender, bookingId, _timerConfiguration.Expiration, logger);
    }
}
