using System.Collections.Generic;

namespace IWent.Persistence.Entities;

/// <summary>
/// Represents the user's order.
/// </summary>
public class Payment
{
    /// <summary>
    /// The unique identifier of the payment.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// The current status of the payment.
    /// </summary>
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// The items in this order.
    /// </summary>
    public ICollection<OrderItem> OrderItems { get; set; } = null!;
}
