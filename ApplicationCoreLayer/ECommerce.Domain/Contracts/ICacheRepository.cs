namespace ECommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string cachekey);
        Task SetAsync(string cachekey, string cacheValue, TimeSpan timeToLive);
    }
}
