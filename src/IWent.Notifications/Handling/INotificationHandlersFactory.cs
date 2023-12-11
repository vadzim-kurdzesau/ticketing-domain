using IWent.Messages;
using IWent.Notifications.Handling.Handlers;

namespace IWent.Notifications.Handling;

internal interface INotificationHandlersFactory
{
    INotificationHandler Create(INotification notification);
}
