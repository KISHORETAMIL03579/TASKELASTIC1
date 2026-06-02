using APPLICATION.DTO;
using DOMAIN.Entities;

namespace APPLICATION.Interfaces;

public interface IProductRepository
{
    Task<string> CreateAsync(Guid id, ProductDTO product);
    Task<ProductDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDTO>> GetByNameAsync(string name);
    Task<bool> UpdateAsync(Guid id, ProductDTO product);
    Task<bool> DeleteAsync(Guid id);
}
