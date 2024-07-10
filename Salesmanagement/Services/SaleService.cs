using Salesmanagement.Models;
using Salesmanagement.Repositories;
using System.Globalization;

namespace Salesmanagement.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISaleRepository _saleRepository;

        public SalesService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            await _saleRepository.AddAsync(sale);
            return sale;
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            return await _saleRepository.GetByIdAsync(id);
        }

        public async Task<Sale> UpdateSaleAsync(int id, Sale sale)
        {
            var existingSale = await _saleRepository.GetByIdAsync(id);
            if (existingSale == null)
            {
                return null;
            }

            existingSale.CustomerName = sale.CustomerName;
            existingSale.Amount = sale.Amount;
            existingSale.SaleDate = sale.SaleDate;
            existingSale.ProductId = sale.ProductId;
            existingSale.RegionId = sale.RegionId;

            await _saleRepository.UpdateAsync(existingSale);

            return existingSale;
        }

        public async Task DeleteSaleAsync(int id)
        {
            await _saleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<decimal> CalculateTotalSalesAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetAllAsync();
            sales = sales.Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate);
            return sales.Sum(s => s.Amount);
        }

        public async Task<Dictionary<string, decimal>> GetSalesTrendsAsync(string interval)
        {
            var sales = await _saleRepository.GetAllAsync();

            var salesGrouped = sales.GroupBy(
                s => interval switch
                {
                    "daily" => s.SaleDate.Date.ToString("yyyy-MM-dd"),
                    "weekly" => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(s.SaleDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString(),
                    "monthly" => new DateTime(s.SaleDate.Year, s.SaleDate.Month, 1).ToString("yyyy-MM"),
                    _ => throw new ArgumentException("Invalid interval")
                }
            ).Select(g => new
            {
                Interval = g.Key,
                TotalSales = g.Sum(s => s.Amount)
            })
            .ToDictionary(x => x.Interval, x => x.TotalSales);

            return salesGrouped;
        }



        public async Task<List<Product>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int count)
        {
            var sales = await _saleRepository.GetAllAsync();
            var topProducts = sales.GroupBy(s => s.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSales = g.Sum(s => s.Amount)
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(count)
                .Select(x => new Product
                {
                    Id = x.ProductId,
                    // Fetch product details from ProductRepository if needed
                })
                .ToList();

            return topProducts;
        }

        public async Task<Dictionary<string, decimal>> GetSalesByRegionAsync()
        {
            var sales = await _saleRepository.GetAllAsync();
            var salesByRegion = sales.GroupBy(s => s.RegionId)
                .Select(g => new
                {
                    RegionId = g.Key,
                    TotalSales = g.Sum(s => s.Amount)
                })
                .ToDictionary(x => x.RegionId.ToString(), x => x.TotalSales);

            return salesByRegion;
        }
    }
}
