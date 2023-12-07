using System.Threading;
using System.Threading.Tasks;

namespace IWent.Services.Notifications;

public interface INotificationClient
{
    Task SendMessageAsync<T>(T message, string queueName, CancellationToken cancellationToken);
}
