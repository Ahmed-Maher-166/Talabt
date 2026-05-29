using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.Core.Models.Order;

namespace Talabat.APIS.Helper
{
    public class ImageOrderUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _config;

        public ImageOrderUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        string IValueResolver<OrderItem, OrderItemDto, string>.Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_config["ApiBaseUrl"]}{source.Product.PictureUrl}";
            return string.Empty;
        }
    }
}
