using AutoMapper;
using ClassLibrary1.DTO.Cart;
using ClassLibrary1.DTO.CartItem;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/cartitems
        [HttpGet]
        public async Task<IActionResult> GetAllCartItems()
        {
            var cartItems = await _unitOfWork.CartItems.GetAllAsync(includeProperties: new Expression<Func<CartItem, object>>[]
            {
        ci => ci.Product // Include Product để lấy giá.
            });

            // Ánh xạ sang DTO
            var cartItemsDto = _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);

            return Ok(cartItemsDto);
        }

        // GET: api/cartitems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id, ci => ci.Cart, ci => ci.Product);
            if (cartItem == null)
                return NotFound(new { message = "Không tìm thấy CartItem." });

            var cartItemDto = _mapper.Map<CartItemDTO>(cartItem);
            return Ok(cartItemDto);
        }

        // POST: api/cartitems
        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItemCreateDTO cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _unitOfWork.Products.GetByIdAsync(cartItemDto.ProductId);
            if (product == null)
                return NotFound($"Product with ID {cartItemDto.ProductId} not found.");

            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _unitOfWork.CartItems.AddAsync(cartItem);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetAllCartItems), null, cartItemDto);
        }

        // PUT: api/cartitems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemUpdateDTO cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                return NotFound($"CartItem with ID {id} not found.");

            _mapper.Map(cartItemDto, cartItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/cartitems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null)
                return NotFound(new { message = "Không tìm thấy CartItem." });

            _unitOfWork.CartItems.Delete(cartItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
