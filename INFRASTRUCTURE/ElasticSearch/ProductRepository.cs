using APPLICATION.DTO;
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

        public async Task<bool> CreateAsync(Guid id, Product product)
        {
            var response = await _client.IndexAsync(product, i => i
                .Index(Index)
                .Id(id.ToString()));

            return response.IsValidResponse;
        }

        public async Task<ProductDTO?> GetByIdAsync(Guid id)
        {
            var response = await _client.GetAsync<ProductDTO>(id.ToString(), g => g.Index(Index));
            return response.Source;
        }

        public async Task<IEnumerable<ProductDTO>> GetByNameAsync(string name)
        {
            //var response = await _client.SearchAsync<ProductDTO>(s => s
            //    .Index("products")
            //    .Query(q => q
            //        .Match(m => m
            //            .Field(f => f.Name)
            //            .Query(name)
            //            .Fuzziness(new Fuzziness("AUTO"))
            //            .PrefixLength(1)
            //        )
            //    )
            //);

            var response = await _client.SearchAsync<ProductDTO>(s => s
                .Index("products")
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            sh => sh.Match(m => m
                                .Field(f => f.Name)
                                .Query(name)
                                .Fuzziness(new Fuzziness("AUTO"))
                            ),
                            sh => sh.Wildcard(w => w
                                .Field(f => f.Name.Suffix("keyword"))
                                .Value($"*{name.ToLower()}*")
                            )
                        )
                    )
                )
            );

            return response.Documents;
        }

        public async Task<bool> PatchAsync(Guid id, ProductPatchDTO product)
        {
            var response = await _client.UpdateAsync<Product, ProductPatchDTO>(
                Index,
                id.ToString(),
                u => u.Doc(product));

            return response.IsValidResponse;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _client.DeleteAsync<Product>(id.ToString(), d => d.Index(Index));
            return response.IsValidResponse;
        }
    }
}