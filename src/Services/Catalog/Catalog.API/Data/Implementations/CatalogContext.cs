using Catalog.API.Data.Interfaces;
using Catalog.API.Data.Seeding;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data.Implementations
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration _configuration;
        public IMongoCollection<Product> Products { get; }
        public CatalogContext(IConfiguration configuration)
        {
            _configuration = configuration;

            var mongoClient = new MongoClient(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = mongoClient.GetDatabase(_configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = database.GetCollection<Product>(_configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            CatalogContextSeed.TryToSeedData(Products); 
        }
    }
}
