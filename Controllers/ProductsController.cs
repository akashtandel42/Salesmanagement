using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salesmanagement.Data;
using Salesmanagement.Models;
using Salesmanagement.Services;

namespace Salesmanagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _logger.LogInformation("Creating new product...");
            var createdProduct = await _productService.CreateProductAsync(product);
            
            _logger.LogInformation("Product created with ID: {ProductId}", createdProduct.Id);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", id);
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }
            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid product model.");
                return BadRequest(ModelState);
            }

            var result = await _productService.UpdateProductAsync(id, product);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);
            _logger.LogInformation("Product deleted successfully");     
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products");

            var products = await _productService.GetAllProductsAsync();

            return Ok(products);
        }
    }
}
