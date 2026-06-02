using APPLICATION.DTO;
using APPLICATION.Interfaces;
using APPLICATION.Services;
using DOMAIN.Entities;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var result = await _service.GetAllAsync();
            if (result == null || !result.Any())
                return NotFound("No products available now");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound($"Product Not Found For the id {id}") : Ok(result);
        }

        [HttpGet("Name")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _service.GetByNameAsync(name);
            return result == null ? NotFound($"Product Not Found For the Name {name}") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            var success = await _service.CreateAsync(product);
            return success ? Ok("Product Created Successfully") : BadRequest("Failed to create product");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, ProductPatchDTO product)
        {
            var existingProduct = await _service.GetByIdAsync(id);

            if (existingProduct != null)
            {

                var result = await _service.PatchAsync(id, product);
            }
            else
            {
                return NotFound($"Product Not Found For the id {id}");
            }

            return Ok("Product Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingProduct = await _service.GetByIdAsync(id);

            if (existingProduct != null)
            {
                var result = await _service.DeleteAsync(id);
            }
            else
            {
                return NotFound($"Product Not Found For the id {id}");
            }

            return Ok("Product Deleted Successfully");
        }
    }
}