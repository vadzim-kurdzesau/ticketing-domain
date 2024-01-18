namespace IWent.BookingTimer.Configuration;

public class BusConfiguration : IBusConfiguration
{
    public string Namespace { get; init; } = null!;

    public string ReceiverQueueName { get; init; } = null!;

    public string SenderQueueName { get; init; } = null!;
}
