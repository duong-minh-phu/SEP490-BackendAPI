using AutoMapper;
using ClassLibrary1.DTO.Order;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(
                includeProperties: new Expression<Func<Order, object>>[] { o => o.User });

            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id, o => o.User);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = _mapper.Map<Order>(orderCreateDto);
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();

            var createdOrder = await _unitOfWork.Orders.GetByIdAsync(order.OrderId, o => o.User);
            var orderDto = _mapper.Map<OrderDto>(createdOrder);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, orderDto);
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDto orderUpdateDto)
        {
            if (!ModelState.IsValid || id != orderUpdateDto.OrderId)
            {
                return BadRequest();
            }

            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            _mapper.Map(orderUpdateDto, existingOrder);
            _unitOfWork.Orders.Update(existingOrder);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
