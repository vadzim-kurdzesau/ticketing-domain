namespace IWent.Messages.Models
{
    /// <summary>
    /// The location where this event is happening.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The county name where this venue is located.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The region where this venue is located.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// The city where this venue is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The street on which this venue is located.
        /// </summary>
        public string Street { get; set; }
    }

}
