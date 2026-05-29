using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specification
{
    public class ProductWithFiltration : BaseSpecification<Product>
    {
        public ProductWithFiltration(ProductSpecSpecification Params)
            : base(p =>
                    (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId) &&
                    (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId) &&
                    (string.IsNullOrEmpty(Params.Name) || p.Name.ToLower().Contains(Params.Name.ToLower()))
                  )
            {

             }
    }
}
