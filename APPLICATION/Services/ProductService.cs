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

        public async Task<bool> PatchAsync(Guid id, ProductPatchDTO product)
        {
            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
                return false;

            // Build merged values (existing + incoming patch)
            var merged = new ProductDTO
            {
                Name = !string.IsNullOrWhiteSpace(product.Name) ? product.Name : existing.Name,
                Price = product.Price.HasValue ? product.Price.Value : existing.Price,
                Stock = product.Stock.HasValue ? product.Stock.Value : existing.Stock
            };

            // Validate merged values using domain entity rules
            var domainProduct = new DOMAIN.Entities.Product
            {
                Id = id,
                Name = merged.Name,
                Price = merged.Price,
                Stock = merged.Stock
            };

            var validationResults = domainProduct.Validate(new ValidationContext(domainProduct));
            var errors = validationResults?.ToList();
            if (errors != null && errors.Count > 0)
            {
                throw new ValidationException(string.Join(", ", errors.Select(x => x.ErrorMessage)));
            }

            // Create a patch DTO containing the merged values and pass to repository
            var mergedPatch = new ProductPatchDTO
            {
                Name = merged.Name,
                Price = merged.Price,
                Stock = merged.Stock
            };

            var result = await _repo.PatchAsync(id, mergedPatch);

            return result;
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
