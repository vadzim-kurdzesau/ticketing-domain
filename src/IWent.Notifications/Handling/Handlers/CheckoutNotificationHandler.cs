using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Messages;
using IWent.Messages.Constants;
using IWent.Messages.Contents;
using IWent.Messages.Models;
using IWent.Notifications.Email;
using IWent.Notifications.Email.Builders.HTML;
using IWent.Notifications.Email.Builders.Templates;
using IWent.Notifications.Email.Configuration;
using Microsoft.Extensions.Logging;

namespace IWent.Notifications.Handling.Handlers;

internal class CheckoutNotificationHandler : INotificationHandler
{
    private readonly IEmailClient _emailClient;
    private readonly IEmailTemplatesStorageFactory _templatesStorageFactory;
    private readonly IEmailClientConfiguration _clientConfiguration;
    private readonly ILogger<CheckoutNotificationHandler> _logger;

    public CheckoutNotificationHandler(
        IEmailClient emailClient,
        IEmailTemplatesStorageFactory templatesStorageFactory,
        IEmailClientConfiguration clientConfiguration,
        ILogger<CheckoutNotificationHandler> logger)
    {
        _emailClient = emailClient;
        _templatesStorageFactory = templatesStorageFactory;
        _clientConfiguration = clientConfiguration;
        _logger = logger;
    }

    public async Task HandleAsync(INotification notification, Microsoft.Azure.WebJobs.ExecutionContext executionContext, CancellationToken cancellationToken)
    {
        if (!notification.Parameters.TryGetValue(NotificationParameterKeys.ReceiverEmail, out var email)
            || !notification.Parameters.TryGetValue(NotificationParameterKeys.ReceiverName, out var receiverName))
        {
            _logger.LogError("Received an invalid '{Checkout}' notification. Either {Email} or {Name} is missing.",
                Operation.Checkout.ToString(),
                NotificationParameterKeys.ReceiverEmail,
                NotificationParameterKeys.ReceiverName);

            return;
        }

        var boughtTickets = (notification.Content as TicketsCheckoutContent)?.Tickets;
        if (boughtTickets == null)
        {
            _logger.LogError("Received an invalid '{Checkout}' notification. The bought tickets are missing.", Operation.Checkout.ToString());
            return;
        }

        var templatesStorage = _templatesStorageFactory.Create(executionContext);
        var messageBuilder = new HtmlEmailMessageBuilder()
            .AddSender(_clientConfiguration.Username, _clientConfiguration.SenderName)
            .AddReceiver(email, receiverName)
            .SetSubject("Your Tickets")
            .SetBody(await BuildNotificationMessageBodyAsync(templatesStorage, boughtTickets, cancellationToken));

        await _emailClient.SendEmailAsync(messageBuilder.Create(), cancellationToken);
    }

    private async ValueTask<string> BuildNotificationMessageBodyAsync(IEmailTemplatesStorage templatesStorage, IEnumerable<Ticket> tickets, CancellationToken cancellationToken)
    {
        var ticketsBody = await templatesStorage.GetTemplateAsync("CheckoutMessageBody.xslt", cancellationToken);
        var messageBodyBuilder = new HtmlEmailBodyBuilder(ticketsBody);
        var ticketsInMessage = new HtmlEmailBodyElement("Tickets");

        messageBodyBuilder.AddElement(ticketsInMessage);
        foreach (var ticket in tickets)
        {
            ticketsInMessage.AddNested(new HtmlEmailBodyElement("Ticket")
                .AddNested(new HtmlEmailBodyElement("EventName", ticket.EventName))
                .AddNested(new HtmlEmailBodyElement("Occurs", ticket.Date.ToString("G"))
                .AddNested(new HtmlEmailBodyElement("Venue", ticket.Address.Street))
                .AddNested(new HtmlEmailBodyElement("SeatNumber", ticket.Number))
                .AddNested(new HtmlEmailBodyElement("Price", ticket.Price.Amount.ToString("#.##")))));
        }

        return messageBodyBuilder.Build();
    }
}
