using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Contracts;
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
        public void Initialize()
        {
            try
            {
                var hasProducts = _dbContext.Products.Any();
                var hasBrands = _dbContext.ProductBrands.Any();
                var hasTypes = _dbContext.ProductTypes.Any();
                if (hasProducts && hasBrands && hasTypes)
                    return;

                if (!hasBrands)
                    SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);

                if (!hasTypes)
                    SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                _dbContext.SaveChanges();

                if (!hasProducts)
                    SeedDataFromJson<Product, int>("roducts.json", _dbContext.Products);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding failed : {ex}");
            }
        }

        private void SeedDataFromJson<T, TKey>(string fileName ,DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            // C:\Users\DELL\Desktop\MyProjectsInVS\E-CommerceSolution\InfrastructureLayer\ECommerce.Persistence\Data\DataSeed\JSONFiles\
            var filePath = @"..\ECommerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File : {fileName} is not exists");

            try
            {
                using var dataStream = File.OpenRead(filePath);

                var data = JsonSerializer.Deserialize<List<T>>(dataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                    dbset.AddRange(data);
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error while reading JSON file : {ex}");
            }
        }
    }
}
