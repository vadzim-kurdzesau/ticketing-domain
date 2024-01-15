namespace IWent.Services.Notifications.Configuration;

public interface IBusConnectionConfiguration
{
    string Namespace { get; set; }

    string QueueName { get; set; }
}