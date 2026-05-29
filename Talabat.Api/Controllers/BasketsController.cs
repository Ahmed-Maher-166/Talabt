using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Models;
using Talabat.Core.Repository;

namespace Talabat.APIS.Controllers
{

    public class BasketsController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;


        public BasketsController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket?>> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            return basket is null ? new CustomerBasket(basketId) : Ok(basket);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);

            var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            if (createdOrUpdatedBasket == null)
                return BadRequest(new ApiResponse(400));

            return Ok(createdOrUpdatedBasket);
        }
        [Authorize]
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string basketId)
        => await _basketRepository.DeleteBasketAsync(basketId);

        }
}
