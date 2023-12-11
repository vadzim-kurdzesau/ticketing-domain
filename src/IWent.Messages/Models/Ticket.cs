using System;

namespace IWent.Messages.Models
{
    public class Ticket
    {
        public string EventName { get; set; }

        public DateTime Date { get; set; }

        public Address Address { get; set; }

        public string SectionName { get; set; }

        public int? RowNumber { get; set; }

        public int Number { get; set; }

        public Price Price { get; set; }
    }
}
