using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Services.DTO;
using IWent.Services.DTO.Orders;
using IWent.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICartService _cartService;

    public OrdersController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("carts/{cartId}")]
    public IEnumerable<OrderItem> GetCartItemsAsync(string cartId)
    {
        return _cartService.GetItemsInCart(cartId);
    }

    [HttpPost("carts/{cartId}")]
    public async Task<IActionResult> AddToCartAsync(string cartId, OrderItem item, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _cartService.AddToCartAsync(cartId, item, cancellationToken));
        }
        catch (ResourseAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
    public IActionResult RemoveFromCartAsync(string cartId, int eventId, int seatId)
    {
        _cartService.RemoveFromCart(cartId, eventId, seatId);
        return Ok();
    }

    [HttpPut("carts/{cartId}/book")]
    public async Task<IActionResult> BookSeatsAsync(string cartId, CancellationToken cancellationToken)
    {
        var paymentInfo = await _cartService.BookSeatsAsync(cartId, cancellationToken);
        return Ok(paymentInfo);
    }
}
