namespace IWent.Persistence.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public string PaymentId { get; set; } = null!;

    public int SeatId { get; set; }

    public int PriceId { get; set; }

    public Payment Payment { get; set; } = null!;

    public Seat Seat { get; set; } = null!;

    public Price Price { get; set; } = null!;
}
