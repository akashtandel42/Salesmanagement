using Salesmanagement.Models;

namespace Salesmanagement.Services
{
    public interface ISalesService
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<Sale> GetSaleByIdAsync(int id);
        Task<Sale> UpdateSaleAsync(int id, Sale sale);
        Task DeleteSaleAsync(int id);
        Task<IEnumerable<Sale>> GetAllSalesAsync();

        Task<decimal> CalculateTotalSalesAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, decimal>> GetSalesTrendsAsync(string interval);
        Task<List<Product>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int count);
        Task<Dictionary<string, decimal>> GetSalesByRegionAsync();
    }
}
