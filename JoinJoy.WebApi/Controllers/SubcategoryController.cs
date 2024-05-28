using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<Subcategory>> GetSubcategories()
        {
            return await _subcategoryService.GetAllSubcategoriesAsync();
        }

        [HttpGet("{categoryId}")]
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryId(int categoryId)
        {
            return await _subcategoryService.GetSubcategoriesByCategoryIdAsync(categoryId);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Subcategory>> GetSubcategory(int id)
        {
            var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return subcategory;
        }

        [HttpPost]
        public async Task<ActionResult<Subcategory>> AddSubcategory(Subcategory subcategory)
        {
            await _subcategoryService.AddSubcategoryAsync(subcategory);
            return CreatedAtAction(nameof(GetSubcategory), new { id = subcategory.Id }, subcategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubcategory(int id, Subcategory subcategory)
        {
            if (id != subcategory.Id)
            {
                return BadRequest();
            }
            await _subcategoryService.UpdateSubcategoryAsync(subcategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubcategory(int id)
        {
            await _subcategoryService.DeleteSubcategoryAsync(id);
            return NoContent();
        }
    }
}
