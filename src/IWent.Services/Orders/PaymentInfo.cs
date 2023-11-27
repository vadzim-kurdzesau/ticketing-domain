using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWent.Services.Orders;

public class PaymentInfo
{
    public string PaymentId { get; set; }

    public PaymentStatus Status { get; set; }
}
