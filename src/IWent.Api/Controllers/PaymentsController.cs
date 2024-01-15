using System.Threading;
using System.Threading.Tasks;
using IWent.Services;
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
    public async Task<IActionResult> GetPaymentInfoAsync(string paymentId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
        {
            return BadRequest("Payment can't be null, empty or a whitespace.");
        }

        return Ok(await _paymentService.GetPaymentInfoAsync(paymentId, cancellationToken));
    }

    [HttpPost("{paymentId}/complete")]
    public async Task<IActionResult> CompleteOrderPaymentAsync(string paymentId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
        {
            return BadRequest("Payment can't be null, empty or a whitespace.");
        }

        await _paymentService.CompletePaymentAsync(paymentId, cancellationToken);
        return Ok();
    }

    [HttpPost("{paymentId}/failed")]
    public async Task<IActionResult> FailOrderPaymentAsync(string paymentId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
        {
            return BadRequest("Payment can't be null, empty or a whitespace.");
        }

        await _paymentService.FailOrderPaymentAsync(paymentId, cancellationToken);
        return Ok();
    }
}
