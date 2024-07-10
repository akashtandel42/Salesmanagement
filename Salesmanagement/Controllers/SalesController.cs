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
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISalesService salesService, ILogger<SalesController> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale(Sale sale)
        {
            _logger.LogInformation("Creating new sale record...");

            var createdSale = await _salesService.CreateSaleAsync(sale);

            _logger.LogInformation("Sale record created with ID: {SaleId}", createdSale.Id);

            return CreatedAtAction(nameof(GetSaleById), new { id = createdSale.Id }, createdSale);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSaleById(int id)
        {
            _logger.LogInformation("Getting sale record with ID: {SaleId}", id);

            var sale = await _salesService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                _logger.LogWarning("Sale record with ID {SaleId} not found", id);
                return NotFound();
            }
            return sale;
        }







        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(int id, [FromBody] Sale sale)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Mismatch between provided ID ({ProvidedId}) and sale ID ({SaleId})", id, sale.Id);
                _logger.LogError("Invalid sale model.");
                return BadRequest(ModelState);
            }

            var result = await _salesService.UpdateSaleAsync(id, sale);

            if (result == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Updating sale record with ID: {SaleId}", sale.Id);
            return Ok(result);
            _logger.LogInformation("Sale record updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _salesService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Deleting sale record with ID: {SaleId}", id);
            await _salesService.DeleteSaleAsync(id);
            _logger.LogInformation("Sale record deleted successfully");
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetAllSales()
        {
            _logger.LogInformation("Fetching all sales records");

            var sales = await _salesService.GetAllSalesAsync();

            return Ok(sales);
        }
    }
}
