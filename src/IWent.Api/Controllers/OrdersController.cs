using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IWent.Api.Models;
using IWent.Api.Models.Cart;
using IWent.Cart;
using Microsoft.AspNetCore.Mvc;

namespace IWent.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public OrdersController(ICartService cartService, IMapper mapper)
    {
        _cartService = cartService;
        _mapper = mapper;
    }

    [HttpGet("carts/{cartId}")]
    public IEnumerable<OrderItem> GetCartItems(string cartId, CancellationToken cancellationToken)
    {
        var userCart = _cartService.GetUserCart(cartId);
        return _mapper.Map<IEnumerable<OrderItem>>(userCart);
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
