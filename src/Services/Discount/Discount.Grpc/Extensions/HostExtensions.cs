using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database");

                    var dbConnectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
                    using var connection = new NpgsqlConnection(dbConnectionString);
                    connection.Open();

                    //the DB is already created using the connection string.

                    using var command = new NpgsqlCommand { Connection = connection };
                    command.CommandText = "DROP TABLE IF EXISTS Coupons";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupons(Id SERIAL PRIMARY KEY,
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupons(ProductName,Description, Amount) 
                                                        VALUES('IPhone X', 'Iphone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupons(ProductName,Description, Amount) 
                                                        VALUES('Samsung 10', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postgresql database");
                }
                catch (Exception ex)
                {

                    logger.LogError(ex, "Error while migrating the postgresql database");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host,retryForAvailability);
                    }
                }

                return host;
            }
        }
    }
}
