using System;
using System.Collections.Generic;
using System.Text;
using IWent.Messages.Models;

namespace IWent.Messages.Contents
{
    public class TicketsCheckoutContent : IMessageContent
    {
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
