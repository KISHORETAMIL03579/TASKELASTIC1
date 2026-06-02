using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace APPLICATION.Interfaces
{
    public interface IProductService
    {
        Task<string> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string id);
    }
}
