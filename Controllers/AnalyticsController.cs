using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salesmanagement.Models;
using Salesmanagement.Services;

namespace Salesmanagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(ISalesService salesService, ILogger<AnalyticsController> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

        [HttpGet("totalsales")]
        public async Task<ActionResult<decimal>> GetTotalSales(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Fetching total sales");
            var totalSales = await _salesService.CalculateTotalSalesAsync(startDate, endDate);
            return Ok(totalSales);
        }

        [HttpGet("salestrends")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetSalesTrends(string interval)
        {
            _logger.LogInformation("Fetching sales trends");
            var salesTrends = await _salesService.GetSalesTrendsAsync(interval);
            return Ok(salesTrends);
        }

        [HttpGet("topproducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetTopSellingProducts(DateTime startDate, DateTime endDate, int count)
        {
            _logger.LogInformation("Fetching top selling products");
            var topProducts = await _salesService.GetTopSellingProductsAsync(startDate, endDate, count);
            return Ok(topProducts);
        }

        [HttpGet("salesbyregion")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetSalesByRegion()
        {
            _logger.LogInformation("Fetching sales by region");
            var salesByRegion = await _salesService.GetSalesByRegionAsync();
            return Ok(salesByRegion);
        }
    }
}
