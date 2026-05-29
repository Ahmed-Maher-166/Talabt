using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repository
{
    public interface IBasketRepository
    {
      public  Task<CustomerBasket> GetBasketAsync(string BasketId);
        public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket Basket);
        public Task<bool> DeleteBasketAsync(string BasketId);
    }
}
