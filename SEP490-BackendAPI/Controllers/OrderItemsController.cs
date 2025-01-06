using AutoMapper;
using ClassLibrary1.DTO.OrderItem;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await _unitOfWork.OrderItems.GetAllAsync(
            includeProperties: new Expression<Func<OrderItem, object>>[] { oi => oi.Order, oi => oi.Product });
            var result = _mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);
            return Ok(result);
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
        {
            var orderItems = await _unitOfWork.OrderItems.GetAllAsync(
                filter: oi => oi.OrderId == orderId,
                includeProperties: new Expression<Func<OrderItem, object>>[] { oi => oi.Order, oi => oi.Product }
            );

            if (orderItems == null || !orderItems.Any())
            {
                return NotFound(new { message = "Không tìm thấy OrderItems với OrderId này." });
            }

            var orderItemsDto = _mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);

            return Ok(orderItemsDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemCreateDTO orderItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderItem = _mapper.Map<OrderItem>(orderItemDto);

            await _unitOfWork.OrderItems.AddAsync(orderItem);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<OrderItemDTO>(orderItem);
            return Ok("create thanh cong");
        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemUpdateDTO orderItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            _mapper.Map(orderItemDto, orderItem);
            _unitOfWork.OrderItems.Update(orderItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            _unitOfWork.OrderItems.Delete(orderItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
