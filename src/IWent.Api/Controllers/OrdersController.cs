using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Services.DTO;
using IWent.Services.DTO.Orders;
using IWent.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _cartService;

    public OrdersController(IOrdersService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("carts/{cartId}")]
    public IEnumerable<OrderItem> GetCartItems(string cartId)
    {
        return _cartService.GetItemsInCart(cartId);
    }

    [HttpPost("carts/{cartId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult RemoveFromCart(string cartId, int eventId, int seatId)
    {
        try
        {
            _cartService.RemoveFromCart(cartId, eventId, seatId);
            return Ok();
        }
        catch (ApiException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("carts/{cartId}/book")]
    public async Task<IActionResult> BookSeats(string cartId, CancellationToken cancellationToken)
    {
        try
        {
            var paymentInfo = await _cartService.BookSeatsAsync(cartId, cancellationToken);
            return Ok(paymentInfo);
        }
        catch (CartIsEmptyException)
        {

            throw;
        }
    }
}
