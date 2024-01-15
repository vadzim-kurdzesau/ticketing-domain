using System.Threading;
using System.Threading.Tasks;
using IWent.Messages;

namespace IWent.Notifications.Handling.Handlers;

internal interface INotificationHandler
{
    Task HandleAsync(INotification notification, Microsoft.Azure.WebJobs.ExecutionContext executionContext, CancellationToken cancellationToken);
}
