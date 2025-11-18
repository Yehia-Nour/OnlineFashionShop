using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string Id)
        {
            var basket = await _database.StringGetAsync(Id);
            if (basket.IsNullOrEmpty)
                return null;
            else 
                return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdate = await _database.StringSetAsync(basket.Id, jsonBasket,
                (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            if (isCreatedOrUpdate)
                return basket;
            else
                return null;
        }

        public async Task<bool> DeleteBasketAsync(string id) => await _database.KeyDeleteAsync(id);
    }
}
