using System;
using System.Collections.Generic;

namespace IWent.Persistence.Entities;

/// <summary>
/// 
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
    /// 
    /// </summary>
    public IEnumerable<OrderItem> OrderItems { get; set; } = Array.Empty<OrderItem>();
}
