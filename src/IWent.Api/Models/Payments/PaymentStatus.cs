namespace IWent.Api.Models.Payments;

/// <summary>
/// Represents the status of a payment.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// The payment is being awaited.
    /// </summary>
    Pending,

    /// <summary>
    /// The payment was successfully completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The payment has failed or was not made.
    /// </summary>
    Failed,
}
