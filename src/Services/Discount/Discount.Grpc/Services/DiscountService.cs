using AutoMapper;
using Discount.Grpc.Entities;
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

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var created = await _discountRepo.CreateDiscountAsync(coupon);

            if (!created)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Discount with ProductName= {coupon.ProductName} was not created."));
            }

            _logger.LogInformation($"Discount successfully created. ProductName: {coupon.ProductName}");

            return _mapper.Map<CouponModel>(coupon);

        }


        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var updated = await _discountRepo.UpdateDiscountAsync(coupon);
            if (!updated)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Discount with ProductName= {coupon.ProductName} was not updated."));
            }

            _logger.LogInformation($"Discount successfully updated. ProductName: {coupon.ProductName}");

            return _mapper.Map<CouponModel>(coupon);

        }

        public override async  Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return new DeleteDiscountResponse { Success = await _discountRepo.DeleteDiscountAsync(request.ProductName) };
        }
    }
}
