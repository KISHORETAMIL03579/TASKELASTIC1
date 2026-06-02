using APPLICATION.DTO;
using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace APPLICATION.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDTO>> GetByNameAsync(string name);
        Task<bool> CreateAsync(ProductDTO product);
        Task<bool> PatchAsync(Guid id, ProductPatchDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}
