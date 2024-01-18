using Azure.Messaging.ServiceBus;

namespace IWent.BookingTimer.Configuration;

internal static class Constants
{
    public const string ServiceBusReceiverName = nameof(ServiceBusReceiver);

    public const string ServiceBusSenderName = nameof(ServiceBusSender);
}
