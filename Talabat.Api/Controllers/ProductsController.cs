using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.APIS.Controllers;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.APIS.Helper;
using Talabat.Core.Models;
using Talabat.Core.Repository;
using Talabat.Core.Specification;

namespace Talabat.Api.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product not found"));
            _unitOfWork.Repository<Product>().Delete(product);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return BadRequest(new ApiResponse(400, "Failed to delete the product"));
            return Ok(new ApiResponse(200, "Product deleted successfully"));
        }

        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductMapper>> AddProduct([FromBody] ProductMapper Product)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid product data"));
            var MappedProduct = _mapper.Map<ProductMapper, Product>(Product);
            _unitOfWork.Repository<Product>().Add(MappedProduct);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return BadRequest(new ApiResponse(400, "Failed to add the product"));
            var productToReturn =_mapper.Map<Product, ProductMapper>(MappedProduct);
            return Ok(productToReturn);
        }


        
        [HttpPut("UpdateProduct")]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<ProductMapper>> UpdateProduct([FromBody] ProductMapper Products)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid product data"));
            var Spec = new ProductWithBrandAndTypeSpecifications(Products.Id);
            var existingProduct = await _unitOfWork.Repository<Product>().GetEntitybySpec(Spec);
            if (existingProduct == null)
                return NotFound(new ApiResponse(404, "Product not found"));
            _mapper.Map(Products, existingProduct);           
            _unitOfWork.Repository<Product>().Update(existingProduct);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return BadRequest(new ApiResponse(400, "Failed to update the product"));
            var productToReturn = _mapper.Map<Product, ProductMapper>(existingProduct);
            return Ok(productToReturn);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Paginations<Product>>>> GetProducts([FromQuery] ProductSpecSpecification ProductSpec)
        {
            var Specification = new ProductWithBrandAndTypeSpecifications(ProductSpec);
            var products = await _unitOfWork.Repository<Product>().GetAll(Specification);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductMapper>>(products);
            var CountSpec = new ProductWithFiltration(ProductSpec);
            int Count = await _unitOfWork.Repository<Product>().GetCount(CountSpec);
            return Ok(new Paginations<ProductMapper>(ProductSpec.PageSize, ProductSpec.Index, Count, MappedProducts));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductMapper>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var products = await _unitOfWork.Repository<Product>().GetEntitybySpec(Spec);
            if (products == null)
                return NotFound(new ApiResponse(404, "Product not found"));
            var MappedProduct = _mapper.Map<Product, ProductMapper>(products);
            return Ok(MappedProduct);

        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAll();
            return Ok(Brands);
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAll();
            return Ok(Types);
        }
    }
}
