using APPLICATION.DTO;
using DOMAIN.Entities;

namespace APPLICATION.Interfaces;

public interface IProductRepository
{
    Task<string> CreateAsync(Guid id, ProductDTO product);
    Task<ProductDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDTO>> GetByNameAsync(string name);
    Task<bool> PatchAsync(Guid id, ProductPatchDTO product);
    Task<bool> DeleteAsync(Guid id);
}
