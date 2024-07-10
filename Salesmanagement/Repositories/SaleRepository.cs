using Microsoft.EntityFrameworkCore;
using Salesmanagement.Data;
using Salesmanagement.Models;

namespace Salesmanagement.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _dbContext;

        public SaleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _dbContext.Sales.FindAsync(id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _dbContext.Sales.ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            _dbContext.Sales.Add(sale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            _dbContext.Entry(sale).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                _dbContext.Sales.Remove(sale);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
