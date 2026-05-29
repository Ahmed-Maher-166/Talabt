using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repository;

namespace Talabt.Reporisitory
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();    
        }

        public async Task<bool> DeleteBasketAsync(string BasketId)
        => await _database.KeyDeleteAsync(BasketId);  
        

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId); 
            return basket.IsNullOrEmpty? null : JsonSerializer.Deserialize<CustomerBasket>(basket);       
        }

        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket Basket)
        {
            var basketJson = JsonSerializer.Serialize(Basket);
            var created = _database.StringSetAsync(Basket.Id, basketJson, TimeSpan.FromDays(30));
            if (!created.Result) return null;
          return GetBasketAsync(Basket.Id);
        }
    }
}
