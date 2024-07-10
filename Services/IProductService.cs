using Salesmanagement.Models;

namespace Salesmanagement.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
