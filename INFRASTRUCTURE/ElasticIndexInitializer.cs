using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;

namespace INFRASTRUCTURE.Elasticsearch
{
    public class ElasticIndexInitializer
    {
        private readonly ElasticsearchClient _client;

        public ElasticIndexInitializer(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task CreateIndexIfNotExists()
        {
            var exists = await _client.Indices.ExistsAsync("products");

            if (!exists.Exists)
            {
                await _client.Indices.CreateAsync("products");
            }
        }
    }
}