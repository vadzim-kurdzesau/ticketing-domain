namespace IWent.Persistence.Entities;

/// <summary>
/// Indicates the current order payment status.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Indicates that a payment is expected.
    /// </summary>
    Pending,

    /// <summary>
    /// Indicates that the order was successfully paid.
    /// </summary>
    Completed,

    /// <summary>
    /// Indicates that the payment was either not completed or wasn't made in time.
    /// </summary>
    Failed,
}
