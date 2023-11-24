namespace IWent.Api.Models.Payments;

/// <summary>
/// Contains information about the requested payment.
/// </summary>
public class PaymentInfo
{
    /// <summary>
    /// The current status of the payment.
    /// </summary>
    public PaymentStatus Status { get; set; }
}
