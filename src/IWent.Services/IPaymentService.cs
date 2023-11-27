using System.Threading;
using System.Threading.Tasks;
using IWent.Services.Orders;

namespace IWent.Services;

public interface IPaymentService
{
    Task<PaymentInfo> GetPaymentInfoAsync(string paymentId, CancellationToken cancellationToken);

    Task CompletePaymentAsync(string paymentId, CancellationToken cancellationToken);

    Task FailOrderPaymentAsync(string paymentId, CancellationToken cancellationToken);
}
