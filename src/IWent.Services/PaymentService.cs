using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Services.DTO.Payments;
using IWent.Services.Exceptions;
using IWent.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IWent.Services;

public class PaymentService : IPaymentService
{
    private readonly EventContext _eventContext;

    public PaymentService(EventContext eventContext)
    {
        _eventContext = eventContext;
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

    public Task CompletePaymentAsync(string paymentId, CancellationToken cancellationToken)
        => ChangePaymentStatusAsync(
            paymentId,
            newPaymentStatus: Persistence.Entities.PaymentStatus.Completed,
            newSeatStatus: Persistence.Entities.SeatStatus.Sold,
            cancellationToken);

    public Task FailOrderPaymentAsync(string paymentId, CancellationToken cancellationToken)
        => ChangePaymentStatusAsync(
            paymentId,
            newPaymentStatus: Persistence.Entities.PaymentStatus.Failed,
            newSeatStatus: Persistence.Entities.SeatStatus.Available,
            cancellationToken);

    private async Task ChangePaymentStatusAsync(
        string paymentId,
        Persistence.Entities.PaymentStatus newPaymentStatus,
        Persistence.Entities.SeatStatus newSeatStatus,
        CancellationToken cancellationToken)
    {
        using (var transaction = _eventContext.Database.BeginTransaction())
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

            payment.Status = newPaymentStatus;
            foreach (var orderItem in payment.OrderItems)
            {
                orderItem.Seat.StateId = newSeatStatus;
            }

            await _eventContext.SaveChangesAsync(cancellationToken);

            transaction.Commit();
        }
    }
}
