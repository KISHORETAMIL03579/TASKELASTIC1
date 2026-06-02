using APPLICATION.Interfaces;
using DOMAIN.Entities;
using Elastic.Clients.Elasticsearch;

namespace INFRASTRUCTURE.ElasticSearch
{
    public class ProductRepository : IProductRepository
    {
        private readonly ElasticsearchClient _client;
        private const string Index = "products";

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<string> CreateAsync(Product product)
        {
            var response = await _client.IndexAsync(product, i => i
                .Index(Index)
                .Id(product.Id));

            return response.Id;
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, g => g.Index(Index));
            return response.Source;
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            var response = await _client.SearchAsync<Product>(s => s
                .Index("products")
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(name)
                        .Fuzziness(new Fuzziness("AUTO"))
                        .PrefixLength(1)
                    )
                )
            );

            return response.Documents;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var response = await _client.UpdateAsync<Product, Product>(
                Index,
                product.Id,
                u => u.Doc(product));

            return response.IsValidResponse;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, d => d.Index(Index));
            return response.IsValidResponse;
        }
    }
}