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

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            var result = await _service.CreateAsync(product);
            return result != null ? Ok("Product Created Succesfully") : BadRequest("Failed to create product");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, ProductDTO product)
        {
            var existingProduct = await _service.GetByIdAsync(id);
            if (existingProduct != null)
            {

                var result = await _service.UpdateAsync(id, product);
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