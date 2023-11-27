using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IWent.Persistence;
using IWent.Services.Exceptions;
using IWent.Services.Orders;
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
            Status = (PaymentStatus)payment.Status,
        };
    }

    public Task CompletePaymentAsync(string paymentId, CancellationToken cancellationToken)
        => ChangePaymentStatusAsync(paymentId, Persistence.Entities.PaymentStatus.Completed, Persistence.Entities.SeatState.Sold, cancellationToken);

    public Task FailOrderPaymentAsync(string paymentId, CancellationToken cancellationToken)
        => ChangePaymentStatusAsync(paymentId, Persistence.Entities.PaymentStatus.Failed, Persistence.Entities.SeatState.Available, cancellationToken);

    private async Task ChangePaymentStatusAsync(
        string paymentId,
        Persistence.Entities.PaymentStatus newPaymentStatus,
        Persistence.Entities.SeatState newSeatState,
        CancellationToken cancellationToken)
    {
        var payment = await _eventContext.Payments.Where(p => p.Id == paymentId)
            .Include(p => p.OrderItems)
            .ThenInclude(p => p.Seat)
            .FirstOrDefaultAsync(cancellationToken);

        if (payment == null)
        {
            throw new ResourceDoesNotExistException($"Payment with the ID '{paymentId}' does not exist.");
        }

        payment.Status = newPaymentStatus;
        foreach (var orderItem in payment.OrderItems)
        {
            orderItem.Seat.State = newSeatState;
        }

        await _eventContext.SaveChangesAsync(cancellationToken);
    }
}
