using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using ProductManagement.Service.Interfaces;
using AutoMapper;
using ProductManagement.Data.Entities;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllProductsAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var product = await _service.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                var productDto = _mapper.Map<ProductDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the product.",
                    Error = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                var result = await _service.AddProductAsync(product);

                if (result.IsSuccess)
                {
                    return Ok(new
                    {
                        Message = "Product added successfully.",
                        ProductId = product.ProductId
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the product.",
                    Error = ex.Message
                });
            }

            return BadRequest(new
            {
                Message = "Failed to add product."
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                product.ProductId = id;

                await _service.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the product.",
                    Error = ex.Message
                });
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the product.",
                    Error = ex.Message
                });
            }
            return Ok();
        }
    }
}
