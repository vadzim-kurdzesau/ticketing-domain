using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Messages;
using IWent.Messages.Constants;
using IWent.Messages.Contents;
using IWent.Messages.Models;
using IWent.Persistence;
using IWent.Persistence.Entities;
using IWent.Services.DTO.Payments;
using IWent.Services.Exceptions;
using IWent.Services.Extensions;
using IWent.Services.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IWent.Services;

public class PaymentService : IPaymentService
{
    private readonly EventContext _eventContext;
    private readonly INotificationClient _notificationClient;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(EventContext eventContext, INotificationClient notificationClient, ILogger<PaymentService> logger)
    {
        _eventContext = eventContext;
        _notificationClient = notificationClient;
        _logger = logger;
    }

    public async Task<PaymentInfo> GetPaymentInfoAsync(string paymentId, CancellationToken cancellationToken)
    {
        var payment = await _eventContext.Payments.FindAsync(new object[] { paymentId }, cancellationToken);
        if (payment == null)
        {
            throw new ResourceDoesNotExistException($"Payment with the ID '{paymentId}' does not exist.");
        }

        return new PaymentInfo
        {
            PaymentId = paymentId,
            Status = payment.Status.ToDTO(),
        };
    }

    public async Task CompletePaymentAsync(string paymentId, CancellationToken cancellationToken)
    {
        var payment = await _eventContext.Payments.Where(p => p.Id == paymentId)
                .Include(p => p.OrderItems)
                .ThenInclude(i => i.Seat)
                .ThenInclude(s => s.Seat)
                .ThenInclude(s => s.Row)
                .ThenInclude(r => r.Section)
                .Include(p => p.OrderItems)
                .ThenInclude(p => p.Event)
                .ThenInclude(e => e.Venue)
                .Include(p => p.OrderItems)
                .ThenInclude(p => p.Price)
                .FirstOrDefaultAsync(cancellationToken);

        if (payment == null)
        {
            throw new ResourceDoesNotExistException($"Payment with the ID '{paymentId}' does not exist.");
        }

        if (payment.Status != Persistence.Entities.PaymentStatus.Pending)
        {
            throw new CannotChangePaymentStatusException("Cannot change status of a non-pending payment.");
        }

        payment.Status = Persistence.Entities.PaymentStatus.Completed;
        foreach (var orderItem in payment.OrderItems)
        {
            orderItem.Seat.StateId = SeatStatus.Sold;
        }

        try
        {
            await _eventContext.SaveChangesAsync(cancellationToken);

            var message = new Notification
            {
                Id = Guid.NewGuid(),
                Operation = Operation.Checkout,
                Parameters = new Dictionary<string, string>()
                {
                    { NotificationParameterKeys.ReceiverEmail, "ticketstestformeonly@mailinator.com" },
                    { NotificationParameterKeys.ReceiverName, "Nick" },
                }.ToImmutableDictionary(),
                Timestamp = DateTime.UtcNow,
                Content = new TicketsCheckoutContent
                {
                    Tickets = payment.OrderItems.Select(ToTicket).ToArray(),
                },
            };

            await _notificationClient.SendMessageAsync(message, "Notifications", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the order payment completion.");
        }
    }

    public async Task FailOrderPaymentAsync(string paymentId, CancellationToken cancellationToken)
    {
        var payment = await _eventContext.Payments.Where(p => p.Id == paymentId)
            .Include(p => p.OrderItems)
            .ThenInclude(p => p.Seat)
            .FirstOrDefaultAsync(cancellationToken);

        if (payment == null)
        {
            throw new ResourceDoesNotExistException($"Payment with the ID '{paymentId}' does not exist.");
        }

        if (payment.Status != Persistence.Entities.PaymentStatus.Pending)
        {
            throw new CannotChangePaymentStatusException("Cannot change status of a non-pending payment.");
        }

        payment.Status = Persistence.Entities.PaymentStatus.Failed;
        foreach (var orderItem in payment.OrderItems)
        {
            orderItem.Seat.StateId = SeatStatus.Available;
        }

        try
        {
            await _eventContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the order payment failing.");
        }
    }

    private static Ticket ToTicket(OrderItem orderItem)
    {
        return new Ticket
        {
            EventName = orderItem.Event.Name,
            Date = orderItem.Event.Date,
            Address = new Address
            {
                Country = orderItem.Event.Venue.Country,
                Region = orderItem.Event.Venue.Region,
                City = orderItem.Event.Venue.City,
                Street = orderItem.Event.Venue.Street,
            },
            SectionName = orderItem.Seat.Seat.Row.Section.Name,
            RowNumber = orderItem.Seat.Seat.Row.Section.SeatType == SeatType.Designated
                ? orderItem.Seat.Seat.RowId
                : null,
            Number = orderItem.Seat.Seat.Number,
            Price = new Messages.Models.Price
            {
                Name = orderItem.Price.Name,
                Amount = orderItem.Price.Amount,
            }
        };
    }
}
