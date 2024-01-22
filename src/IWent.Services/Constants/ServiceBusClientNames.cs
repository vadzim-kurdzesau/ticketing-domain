namespace IWent.Services.Constants;

public static class ServiceBusClientNames
{
    public const string DefaultBusClient = "Default";

    public const string NotificationsSender = nameof(NotificationsSender);

    public const string ExpiredTimersReceiver = nameof(ExpiredTimersReceiver);
}
