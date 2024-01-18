namespace IWent.Services.Notifications.Configuration;

public interface IBusConnectionConfiguration
{
    string Namespace { get; init; }

    string NotificationsQueueName { get; init; }

    string ExpiredTimersQueueName { get; init; }
}