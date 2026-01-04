using HandmadeStore.Application.DTOs;
using HandmadeStore.Application.DTOs.Product;
using HandmadeStore.Application.Interfaces.Service;
using HandmadeStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandmadeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ===================== CREATE =====================
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        
        public async Task<ActionResult<GetProductDto>> Create([FromBody] CreateProductDto dto)
        {
            var product = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id, name = product.Name, description = product.Description }, product);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<GetProductDto>> GetProductByName(string name)
        {
            var product = await _productService.GetProductByNameAsync(name);
            return Ok(product);
        }

        // ===================== GET ALL ACTIVE =====================
        [HttpGet("active")]
        public async Task<ActionResult<IReadOnlyList<GetProductDto>>> GetAllActive()
        {
            var products = await _productService.GetAllActiveProductsAsync();
            return Ok(products);
        }

        // ===================== GET BY ID =====================
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // ===================== GET WITH FILTER =====================
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetProductDto>>> GetFiltered([FromQuery] ProductQueryParams query)
        {
            var products = await _productService.GetProductsAsync(query);
            return Ok(products);
        }

        // ===================== UPDATE =====================
        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                await _productService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ===================== DELETE =====================
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
