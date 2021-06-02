using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepo;
        public DiscountController(IDiscountRepository discountRepo)
        {
            _discountRepo = discountRepo;
        }

        [HttpGet("{productName}", Name ="GetDiscount")]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscountAsync(string productName)
        {
            return Ok(await _discountRepo.GetDiscountAsync(productName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Coupon>> CreateDiscountAsync([FromBody]Coupon coupon)
        {
            var created = await _discountRepo.CreateDiscountAsync(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateDiscountAsync([FromBody] Coupon coupon)
        {
            return Ok(await _discountRepo.UpdateDiscountAsync(coupon));
        }

        [HttpDelete("{productName}",Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscountAsync(string productName)
        {
            return Ok(await _discountRepo.DeleteDiscountAsync(productName));
        }
    }
}
