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

        public async Task<string> CreateAsync(Product product)
        {
            Validate(product);
            return await _repo.CreateAsync(product);
        }

        public Task<Product?> GetByIdAsync(string id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Product>> GetByNameAsync(string name)
       => _repo.GetByNameAsync(name);

        public Task<bool> UpdateAsync(Product product)
        {
            Validate(product);
            return _repo.UpdateAsync(product);
        }

        public Task<bool> DeleteAsync(string id)
            => _repo.DeleteAsync(id);

        private void Validate(Product product)
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
