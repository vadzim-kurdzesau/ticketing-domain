namespace IWent.BookingTimer.Configuration;

public interface IBusConfiguration
{
    string Namespace { get; init; }

    string ReceiverQueueName { get; init; }

    string SenderQueueName { get; init; }
}