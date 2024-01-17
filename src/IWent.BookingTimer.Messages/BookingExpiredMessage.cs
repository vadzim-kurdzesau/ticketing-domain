using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWent.BookingTimer.Messages;

public class BookingExpiredMessage
{
    public string BookingId { get; set; }
}
