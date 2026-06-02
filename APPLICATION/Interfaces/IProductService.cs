using APPLICATION.DTO;
using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace APPLICATION.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateAsync(ProductDTO product);
        Task<ProductDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDTO>> GetByNameAsync(string name);
        Task<bool> UpdateAsync(Guid id, ProductDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}
