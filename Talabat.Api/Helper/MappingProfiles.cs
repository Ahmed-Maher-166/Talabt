using AutoMapper;
using Talabat.Api.DTOS;
using Talabat.APIS.DTOS;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;

namespace Talabat.APIS.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductMapper>()
                .ForMember(d => d.ProductBrand,
                    o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType,
                    o => o.MapFrom(s => s.ProductType.Name))
               .ForMember(d => d.PictureUrl,
                    o => o.MapFrom<ImageProductUrlResolver>());
            CreateMap<ProductMapper, Product>()
                .ForMember(d => d.ProductBrand, o => o.Ignore())
                .ForMember(d => d.ProductType, o => o.Ignore());
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, Basket>().ReverseMap();
            CreateMap<AddressDto, Talabat.Core.Models.Order.Address>();
            CreateMap <AddressDto,Talabat.Core.Models.Identity.Address>().ReverseMap();
            CreateMap<Order, OrderToReturnToDTO>()
               .ForMember(d => d.DeliveryMethod,
                   o => o.MapFrom(s => s.DeliveryMethod.ShortName))
               .ForMember(d => d.DeliveryMethod,
                   o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId,
                o => o.MapFrom(s => s.Product.ProductId))
            .ForMember(d => d.ProductName,
                o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ImageOrderUrlResolver>());
            CreateMap < DeliveryMethodsDto, DeliveryMethod>();
        }
    }
}
