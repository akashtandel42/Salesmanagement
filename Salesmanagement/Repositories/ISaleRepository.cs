using Salesmanagement.Models;

namespace Salesmanagement.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(int id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(int id);
    }
}
