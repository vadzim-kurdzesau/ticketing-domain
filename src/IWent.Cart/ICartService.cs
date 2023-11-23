using System.Collections.Generic;
using IWent.Cart.Models;

namespace IWent.Cart;

public interface ICartService
{
    IEnumerable<CartItem> GetUserCart(string cartId);

    void AddToCart(string cartId, CartItem cartItem);
}
