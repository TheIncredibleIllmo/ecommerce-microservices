using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscountAsync(string productName);
        Task<bool> CreateDiscountAsync(Coupon coupon);
        Task<bool> UpdateDiscountAsync(Coupon coupon);
        Task<bool> DeleteDiscountAsync(string productName);
    }
}
