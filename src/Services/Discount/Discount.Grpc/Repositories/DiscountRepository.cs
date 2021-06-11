using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _dbConnectionString;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dbConnectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {

            try
            {
            using var connection = new NpgsqlConnection(_dbConnectionString);
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                    ("SELECT * FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName });

                return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            }
            catch (Exception ex)
            {

                var m = ex.Message;
            }

            return null;

        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_dbConnectionString);

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupons (ProductName,Description,Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }
        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_dbConnectionString);

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupons SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id=coupon.Id });

            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection(_dbConnectionString);

            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupons WHERE ProductName = @ProductName",
                new { ProductName = productName });

            return affected > 0;
        }

    }
}
