using System.Threading;
using System.Threading.Tasks;
using IWent.Services.DTO.Payments;

namespace IWent.Services;

/// <summary>
/// Provides functionality for API to operate with payments.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Gets the information about payment with the specified <paramref name="paymentId"/>.
    /// </summary>
    Task<PaymentInfo> GetPaymentInfoAsync(string paymentId, CancellationToken cancellationToken);

    /// <summary>
    /// Completes the pending payment with the specified <paramref name="paymentId"/>.
    /// </summary>
    Task CompletePaymentAsync(string paymentId, CancellationToken cancellationToken);

    /// <summary>
    /// Fails the pending payment with the specified <paramref name="paymentId"/>.
    /// </summary>
    Task FailOrderPaymentAsync(string paymentId, CancellationToken cancellationToken);
}
