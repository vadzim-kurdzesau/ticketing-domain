namespace IWent.Services.Notifications.Configuration;

public class BusConnectionConfiguration : IBusConnectionConfiguration
{
    public string Namespace { get; set; }

    public string QueueName { get; set; }
}
