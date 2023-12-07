namespace IWent.Messages.Models
{
    /// <summary>
    /// The price of the seat.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// The name of the price option.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of a price.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
