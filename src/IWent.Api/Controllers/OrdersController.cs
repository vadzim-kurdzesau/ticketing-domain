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
    public IEnumerable<OrderItem> GetCartItems(string cartId)
    {
        return _cartService.GetItemsInCart(cartId);
    }

    [HttpPost("carts/{cartId}")]
    public IActionResult AddToCart(string cartId, OrderItem item)
    {
        try
        {
            return Ok(_cartService.AddToCart(cartId, item));
        }
        catch (CartAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
    public IActionResult RemoveFromCart(string cartId, int eventId, int seatId)
    {
        _cartService.RemoveFromCart(cartId, eventId, seatId);
        return Ok();
    }

    [HttpPut("carts/{cartId}/book")]
    public async Task<IActionResult> BookSeats(string cartId, CancellationToken cancellationToken)
    {
        var paymentInfo = await _cartService.BookSeatsAsync(cartId, cancellationToken);
        return Ok(paymentInfo);
    }
}
