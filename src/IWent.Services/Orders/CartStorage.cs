using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWent.Services.Orders;

public class CartStorage : ICartStorage
{
    public UserCart Get(string cartId)
    {
        throw new NotImplementedException();
    }

    public UserCart GetOrCreate(string cartId)
    {
        throw new NotImplementedException();
    }

    public void Remove(string cartId)
    {
        throw new NotImplementedException();
    }
}
