using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.ProductDto;
using UserManagement.Application.Interfaces;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper) 
        {
            _productService = productService;
            _mapper = mapper;
        }


        [HttpPost]
        [HasPermission("Product Creation")]
        public async Task<IActionResult> CreateBody([FromBody] CreateProduct product)
        {
            if (product == null)
                return BadRequest("Invalid product data.");

            try
            {
                var result = await _productService.CreateProduct(product);
                return Ok(result);
            }
            catch (ArgumentException ex) //handles request body errors
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the product.", Details = ex.Message });
            }
        }

        [HttpGet]
        //[HasPermission("Product View")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
        
        [HttpPut("{id}")]
        [HasPermission("Product Update")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductDto dto)
        {
            try
            {
                var product = await _productService.UpdateProduct(id, dto);

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
