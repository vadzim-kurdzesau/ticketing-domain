using System.Collections.Generic;
using IWent.Messages.Models;

namespace IWent.Messages
{
    public class TicketsBoughtMessage
    {
        public string PaymentId { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
