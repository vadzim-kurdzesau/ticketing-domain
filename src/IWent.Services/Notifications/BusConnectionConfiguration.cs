namespace IWent.Services.Notifications.Configuration;

public class BusConnectionConfiguration : IBusConnectionConfiguration
{
    public string Namespace { get; init; } = null!;

    public string NotificationsQueueName { get; init; } = null!;

    public string BookingTimersQueueName { get; init; } = null!;

    public string ExpiredTimersQueueName { get; init; } = null!;
}
