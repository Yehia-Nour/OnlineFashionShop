using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection) 
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string cachekey)
        {
            var cacheValue = await _database.StringGetAsync(cachekey);
            return cacheValue.IsNullOrEmpty ? null : cacheValue.ToString();
        }

        public async Task SetAsync(string cachekey, string cacheValue, TimeSpan timeToLive)
        {
            await _database.StringSetAsync(cachekey, cacheValue, timeToLive);
        }
    }
}
