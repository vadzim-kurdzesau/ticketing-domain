namespace IWent.Notifications.Messaging;

public interface IMessageQueueConfiguration
{
    string QueueName { get; init; }

    string QueueNamespace { get; init; }
}