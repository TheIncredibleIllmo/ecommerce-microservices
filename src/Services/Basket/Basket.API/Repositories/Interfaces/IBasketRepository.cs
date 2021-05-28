using Basket.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart shoppingCart);
        Task DeleteBasketAsync(string userName);

    }
}
