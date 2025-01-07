using AutoMapper;
using ClassLibrary1.DTO.Cart;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id, c => c.CartItems, c => c.User);
            if (cart == null)
                return NotFound(new { message = "Không tìm thấy giỏ hàng." });
            

            var cartDto = _mapper.Map<CartDTO>(cart);
            return Ok(cartDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CartDTO cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = _mapper.Map<Cart>(cartDto);
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetCartById), new { id = cart.CartId }, cartDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartDTO cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                return NotFound(new { message = "Không tìm thấy giỏ hàng." });

            _mapper.Map(cartDto, cart);
            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                return NotFound(new { message = "Không tìm thấy giỏ hàng." });

            _unitOfWork.Carts.Delete(cart);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
