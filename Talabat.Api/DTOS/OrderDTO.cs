using System.ComponentModel.DataAnnotations;
using Talabat.Core.Models.Order;

namespace Talabat.APIS.DTOS
{
    public class OrderDTO
    {
        [Required]
        public AddressDto ShippingAddress { get; set; }
        [Required]

        public int DeliveryMethod { get; set; }
        [Required]
        public string BasketId { get; set; }
    }
}
