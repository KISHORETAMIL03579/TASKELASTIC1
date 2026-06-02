using DOMAIN.Entities;

namespace APPLICATION.Interfaces;

public interface IProductRepository
{
    Task<string> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetByNameAsync(string name);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
}
