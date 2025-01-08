using AutoMapper;
using ClassLibrary1.DTO.Product;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _unitOfWork.Products.GetAllAsync(includeProperties: p => p.Category);
            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productDtos);
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, p => p.Category);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(CreateProductDTO createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            product.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();

            var createdProductDTO = _mapper.Map<ProductDTO>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, createdProductDTO);
        }

        // PUT: api/Products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDTO updateProductDTO)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(updateProductDTO, product);
            product.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
