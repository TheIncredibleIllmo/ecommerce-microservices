using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    // we are not inheriting from the proto
    // service class due to it is a client not a server.
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }

        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            return await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
            {
                ProductName = productName
            });
        }
    }
}
