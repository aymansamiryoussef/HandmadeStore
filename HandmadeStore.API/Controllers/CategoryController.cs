using HandmadeStore.Application.DTOs.Category;
using HandmadeStore.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandmadeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            var result = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id ,Name=result.Name}, result);
        }

        
        [HttpGet("Categories")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // ================= GET BY ID =================
        [HttpGet("Category/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        // ================= GET BY NAME =================
        [HttpGet("Category/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var category = await _categoryService.GetByNameAsync(name);

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }


        // ================= UPDATE =================
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, CategoryDto dto)
        {
            await _categoryService.UpdateAsync(id, dto);
            return NoContent();
        }

        // ================= DELETE (SOFT DELETE) =================
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

    }
}
