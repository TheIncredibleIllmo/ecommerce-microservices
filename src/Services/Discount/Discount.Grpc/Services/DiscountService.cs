using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepo;
        private readonly IMapper _mapper;

        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository discountRepo, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepo = discountRepo ?? throw new ArgumentException(nameof(discountRepo));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepo.GetDiscountAsync(request.ProductName);
            if (coupon == null) throw new RpcException(new Status(StatusCode.NotFound,$"Discount with ProductName= {request.ProductName} was not found."));

            _logger.LogInformation($"Discount retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

            return _mapper.Map<CouponModel>(coupon);
        }

    }
}
