using APPLICATION.DTO;
using APPLICATION.Interfaces;
using DOMAIN.Entities;
using System.ComponentModel.DataAnnotations;

namespace APPLICATION.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> CreateAsync(ProductDTO product)
        {
            Validate(product);
            Guid id = Guid.NewGuid();
            return await _repo.CreateAsync(id, product);
        }

        public Task<ProductDTO?> GetByIdAsync(Guid id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task<IEnumerable<ProductDTO>> GetByNameAsync(string name)
       => _repo.GetByNameAsync(name);

        public Task<bool> UpdateAsync(Guid id, ProductDTO product)
        {
            Validate(product);
            return _repo.UpdateAsync(id, product);
        }

        public Task<bool> DeleteAsync(Guid id)
            => _repo.DeleteAsync(id);

        private void Validate(ProductDTO product)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(product);

            if (!Validator.TryValidateObject(product, context, results, true))
            {
                throw new ValidationException(
                    string.Join(", ", results.Select(x => x.ErrorMessage))
                );
            }
        }
    }
}
