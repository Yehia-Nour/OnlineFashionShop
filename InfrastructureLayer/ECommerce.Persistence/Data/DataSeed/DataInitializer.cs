using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var hasProducts = await _dbContext.Products.AnyAsync();
                var hasBrands = await _dbContext.ProductBrands.AnyAsync();
                var hasTypes = await _dbContext.ProductTypes.AnyAsync();
                var hasDeliveryMethod = await _dbContext.DeliveryMethods.AnyAsync();
                if (hasProducts && hasBrands && hasTypes && hasDeliveryMethod)
                    return;

                if (!hasBrands)
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands);

                if (!hasTypes)
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                await _dbContext.SaveChangesAsync();

                if (!hasProducts)
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);

                if (!hasDeliveryMethod)
                    await SeedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbContext.DeliveryMethods);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding failed : {ex}");
            }
        }

        private async Task SeedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            var filePath = @"..\..\InfrastructureLayer\ECommerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File : {fileName} is not exists");

            try
            {
                using var dataStream = File.OpenRead(filePath);

                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                    await dbset.AddRangeAsync(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading JSON file : {ex}");
            }
        }
    }
}
