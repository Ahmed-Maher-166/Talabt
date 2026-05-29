using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.Core.Models;

namespace Talabat.APIS.Helper
{
    public class ImageProductUrlResolver : IValueResolver<Product, ProductMapper, string>
    {
        private readonly IConfiguration _config;

        public ImageProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        string IValueResolver<Product, ProductMapper, string>.Resolve(Product source, ProductMapper destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
               return $"{_config["ApiBaseUrl"]}{source.PictureUrl}";        
            return string.Empty;
        }
    }
}
