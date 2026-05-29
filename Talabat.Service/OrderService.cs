using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repository;
using Talabat.Core.Service;

using Talabat.Core.Models.Order;
using Order = Talabat.Core.Models.Order.Order;
using Talabat.Core.Specification.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IBasketRepository basketRepo,
           IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string BasketId, int DeliveryMethod, Address Address)
        {
            var basket = await _basketRepo.GetBasketAsync(BasketId);
            var orderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
               foreach (var item in basket.Items)
                {
                    var productItem = await _unitOfWork.Repository<Product>().GetById(item.Id);
                    var productItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, productItem.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }       
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetById(DeliveryMethod);
            var Spec = new OrderSpecification(buyerEmail);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntitybySpec(Spec);
                if (existingOrder != null)
                {
                    _unitOfWork.Repository<Order>().Delete(existingOrder);
                    await _unitOfWork.CompleteAsync();
                 }
            var order = new Order(buyerEmail, Address, deliveryMethod, orderItems, subtotal );
            _unitOfWork.Repository<Order>().Add(order);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }
        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var Spec = new OrderSpecification(buyerEmail , orderId);
            var users = await _unitOfWork.Repository<Order>().GetEntitybySpec(Spec);
            return users;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {   
            var Spec = new OrderSpecification(buyerEmail);
            var users = await _unitOfWork.Repository<Order>().GetAll(Spec);
            return  users;
        }
    }
}