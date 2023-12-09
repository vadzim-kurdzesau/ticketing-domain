namespace IWent.Notifications.Messaging;

public class MessageQueueConfiguration : IMessageQueueConfiguration
{
    public string QueueName { get; init; } = null!;

    public string QueueNamespace { get; init; } = null!;
}
