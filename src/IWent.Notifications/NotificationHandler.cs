using System;
using System.Threading;
using System.Threading.Tasks;
using IWent.Messages;
using IWent.Notifications.Email.Builders;
using IWent.Notifications.Handling;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IWent.Notifications;

internal class NotificationHandler
{
    private readonly INotificationHandlersFactory _handlersFactory;
    private readonly ILogger<NotificationHandler> _logger;

    public NotificationHandler(INotificationHandlersFactory handlersFactory, ILogger<NotificationHandler> logger)
    {
        _handlersFactory = handlersFactory;
        _logger = logger;
    }

    [FunctionName(nameof(Handle))]
    public async Task Handle([ServiceBusTrigger("Notifications", Connection = "ServiceBusConnection")] string notificationsItem, string messageId, Microsoft.Azure.WebJobs.ExecutionContext executionContext)
    {
        _logger.LogInformation("Received a message with the ID '{ID}'.", messageId);
        if (Constants.AppDirectoryPath == null)
        {
            // TODO: temporary workaround
            Constants.AppDirectoryPath = executionContext.FunctionAppDirectory;
        }

        try
        {
            var notification = JsonConvert.DeserializeObject<Notification>(notificationsItem, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (notification == null)
            {
                _logger.LogWarning("Received message with the ID '{ID}' is not of a '{Notification}' type.", nameof(Notification));
                return;
            }

            var handler = _handlersFactory.Create(notification);
            await handler.HandleAsync(notification, executionContext, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown during the {Handler} execution: {Message}", nameof(NotificationHandler), ex.Message);
            return;
        }

        _logger.LogInformation("Successfully handled the message with the ID '{ID}'.", messageId);
    }
}
