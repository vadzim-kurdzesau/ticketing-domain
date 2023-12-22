namespace IWent.Services.DTO.Events;

/// <summary>
/// Represents a price option the seat can be sold for.
/// </summary>
public class PriceOption
{
    /// <summary>
    /// The unique identifier of the price.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the price option.
    /// </summary>
    public string Name { get; set; } = null!;
}
