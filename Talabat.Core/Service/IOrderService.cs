using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order;

namespace Talabat.Core.Service
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrderAsync(string buyerEmail, string BasketId, int DeliveryMethod, Address Address);
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
    }
}
