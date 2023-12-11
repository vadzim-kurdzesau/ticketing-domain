using System;
using IWent.Messages;
using IWent.Messages.Models;
using IWent.Notifications.Handling.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace IWent.Notifications.Handling;

internal class NotificationHandlersFactory : INotificationHandlersFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationHandlersFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotificationHandler Create(INotification notification)
    {
        return notification.Operation switch
        {
            Operation.Checkout => _serviceProvider.GetRequiredService<CheckoutNotificationHandler>(),
            _ => throw new InvalidOperationException(),
        };
    }
}
