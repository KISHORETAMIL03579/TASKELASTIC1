using APPLICATION.DTO;
using DOMAIN.Entities;

namespace APPLICATION.Interfaces;

public interface IProductRepository
{
    Task<List<ProductDTO>> GetAllAsync();
    Task<ProductDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDTO>> GetByNameAsync(string name);
    Task<bool> CreateAsync(Guid id, Product product);
    Task<bool> PatchAsync(Guid id, ProductPatchDTO product);
    Task<bool> DeleteAsync(Guid id);
}
