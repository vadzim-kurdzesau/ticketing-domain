using System.Net.Mail;
using Azure.Messaging.ServiceBus;
using IWent.Messages;
using IWent.Notifications.Email;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace IWent.Notifications;

public class NotificationsListener : BackgroundService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly IEmailClient _emailClient;

    public NotificationsListener(ServiceBusClient serviceBusClient, IEmailClient emailClient)
    {
        _serviceBusClient = serviceBusClient;
        _emailClient = emailClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = _serviceBusClient.CreateProcessor(queueName: "Email", new ServiceBusProcessorOptions());

        try
        {
            processor.ProcessMessageAsync += MessageHandler;

            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                // Keep background service alive
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
        finally
        {
            await processor.DisposeAsync();
        }
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var json = args.Message.Body.ToString();
        var ticketsMessage = JsonConvert.DeserializeObject<TicketsBoughtMessage>(json);
        if (ticketsMessage == null)
        {
            return;
        }

        var message = new TicketsEmailMessageBuilder();
        foreach (var ticket in ticketsMessage.Tickets)
        {
            message.AddTicket(ticket);
        }

        await _emailClient.SendEmailAsync(message.Create(), CancellationToken.None);

        await args.CompleteMessageAsync(args.Message);
    }

    // handle any errors when receiving messages
    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}
