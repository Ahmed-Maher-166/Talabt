using Talabat.Core.Models.Order;

namespace Talabat.APIS.DTOS
{
    public class OrderToReturnToDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public decimal DeliveryCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }
      
    }
}
