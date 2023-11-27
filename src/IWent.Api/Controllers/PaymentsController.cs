using System.Threading;
using System.Threading.Tasks;
using IWent.Services;
using IWent.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public Task<PaymentInfo> GetPaymentInfo(string paymentId, CancellationToken cancellationToken)
    {
        return _paymentService.GetPaymentInfoAsync(paymentId, cancellationToken);
    }

    [HttpPost("{paymentId}/complete")]
    public async Task<IActionResult> CompleteOrderPayment(string paymentId, CancellationToken cancellationToken)
    {
        await _paymentService.CompletePaymentAsync(paymentId, cancellationToken);
        return Ok();
    }

    [HttpPost("{paymentId}/failed")]
    public async Task<IActionResult> FailOrderPayment(string paymentId, CancellationToken cancellationToken)
    {
        await _paymentService.FailOrderPaymentAsync(paymentId, cancellationToken);
        return Ok();
    }
}
