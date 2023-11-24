using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IWent.Api.Models;
using IWent.Api.Models.Payments;
using IWent.Cart;
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
    public IEnumerable<OrderItem> GetCartItems(string cartId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("carts/{cartId}")]
    public Task<CartState> AddToCart(string cartId, CartSeat item, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("carts/{cartId}/events/{eventId}/seats/{seatId}")]
    public Task<IActionResult> RemoveFromCart(string cartId, int eventId, int seatId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("carts/{cartId}/book")]
    public Task<IActionResult> BookSeats(string cartId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
