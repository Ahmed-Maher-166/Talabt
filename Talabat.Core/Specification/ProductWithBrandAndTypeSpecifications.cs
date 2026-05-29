using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Talabat.Core.Models;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        // Get All with filtering + sorting
        public ProductWithBrandAndTypeSpecifications(ProductSpecSpecification Params)
            : base(p =>
                (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId) &&
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId) &&
                (string.IsNullOrEmpty(Params.Name) || p.Name.ToLower().Contains(Params.Name.ToLower()))
            )
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddByASC(p => p.Price);
                        break;

                    case "PriceDesc":
                        AddByDSC(p => p.Price);
                        break;

                    case "NameAsc":
                        AddByASC(p => p.Name);
                        break;

                    case "NameDesc":
                        AddByDSC(p => p.Name);
                        break;

                    default:
                        AddByASC(p => p.Name);
                        break;
                }
            }
            ApplyPagination(Params.PageSize, Params.PageSize * (Params.Index - 1));
        }

        // Get By Id
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }
    }
}