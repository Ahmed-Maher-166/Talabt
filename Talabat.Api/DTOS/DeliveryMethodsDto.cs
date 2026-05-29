using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.DTOS
{
    public class DeliveryMethodsDto
    {
        [Required]
        public string ShortName { get; set; }
        [Required]

        public string DeliveryTime { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public decimal Cost { get; set; }
    }
}
