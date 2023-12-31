﻿using System.Collections.Generic;

namespace IWent.Persistence.Entities;

/// <summary>
/// Represents an offer single or multiple seats must be sold for.
/// </summary>
public class Price
{
    /// <summary>
    /// The unique identifier of the price.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the price option.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The value of a price.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The list of seats with this price offer.
    /// </summary>
    public IEnumerable<EventSeat> Seats { get; set; } = null!;
}
