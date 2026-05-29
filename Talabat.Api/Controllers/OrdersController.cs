using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.DTOS;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Models.Order;
using Talabat.Core.Repository;
using Talabat.Core.Service;

namespace Talabat.APIS.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IOrderService orderService,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO OrderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Core.Models.Order.Address>(OrderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(email, OrderDto.BasketId, OrderDto.DeliveryMethod, MappedAddress);
            if (order == null) return BadRequest(new ApiResponse(400, "Failed to create order"));
            return Ok(order);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnToDTO>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(email);
            if (orders == null || !orders.Any()) return NotFound(new ApiResponse(404, "No orders found for the user"));
            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnToDTO>>(orders);
            return Ok(mappedOrders);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnToDTO>> GetOrderByIdForUser(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id, email);
            if (order == null) return NotFound(new ApiResponse(404, "Order not found"));
            var mappedOrder = _mapper.Map<Order, OrderToReturnToDTO>(order);
            return Ok(mappedOrder);
        }

        [Authorize]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAll();
            if (DeliveryMethods == null) return NotFound(new ApiResponse(404, "No delivery methods found"));
            return Ok(DeliveryMethods);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethodsDto>> AddDeliveryMethod([FromBody] DeliveryMethodsDto deliveryMethodsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid delivery method data"));

            var deliveryMethod = _mapper.Map<DeliveryMethodsDto, DeliveryMethod>(deliveryMethodsDto);
            _unitOfWork.Repository<DeliveryMethod>().Add(deliveryMethod);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Failed to add delivery method"));
            return Ok(deliveryMethod);
        }
        
    }
}
