using System;
using System.Threading;
using System.Threading.Tasks;
using IWent.Api.Models.Payments;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    [HttpGet]
    public Task<PaymentInfo> GetPaymentInfo(string paymentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{paymentId}/complete")]
    public Task<IActionResult> CompleteOrderPayment(string paymentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{paymentId}/failed")]
    public Task<IActionResult> FailOrderPayment(string paymentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
