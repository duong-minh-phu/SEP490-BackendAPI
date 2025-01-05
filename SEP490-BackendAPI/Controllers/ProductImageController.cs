using AutoMapper;
using ClassLibrary1.DTO.ProductImage;
using ClassLibrary1.Interface;
using ClassLibrary1.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SEP490_BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public ProductImagesController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductImage(IFormFile file, int productId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "product_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = uploadResult.SecureUrl.ToString(),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.ProductImages.AddAsync(productImage);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetProductImagesByProductId), new { productId }, productImage);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductImagesByProductId(int productId)
        {
            var images = await _unitOfWork.ProductImages.GetAllAsync(
                pi => pi.ProductId == productId
            );

            return Ok(images);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductImages()
        {
            var productImages = await _unitOfWork.ProductImages.GetAllAsync();
            var productImageDtos = _mapper.Map<IEnumerable<ProductImageDTO>>(productImages);
            return Ok(productImageDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(int id, IFormFile file)
        {
            var image = await _unitOfWork.ProductImages.GetAllAsync(pi => pi.ImageId == id);
            var productImage = image.FirstOrDefault();
            if (productImage == null)
                return NotFound();

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "product_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            productImage.ImageUrl = uploadResult.SecureUrl.ToString();
            productImage.CreatedDate = DateTime.UtcNow;

            _unitOfWork.ProductImages.Update(productImage);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        
        
    }
}
